using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using UnityEngine;

public class NetManager : InstanceBase<NetManager>
{
    public enum NetStateEvent
    {
        Connected,
        Disconnected,
        ConnectUnStable, // 不稳定
        ConnectedButMessageFailed, // 连接成功但是消息发送失败
    }

    protected Socket m_socket;
    protected string socketIPPort;
    protected ByteBuffer buffer;
    protected ByteBuffer tempBuffer;

    protected ByteBuffer sendBuffer;

    protected Thread sendThread = null;
    protected Thread recvThread = null;

    protected NetPool sendPool;
    protected NetPool recvPool;

    protected ManualResetEvent connectEvent = new ManualResetEvent(false);

    protected ManualResetEvent closeEvent = new ManualResetEvent(false);

    protected Dictionary<int, NetPacket> timeoutPacket = new Dictionary<int, NetPacket>(10);

    public delegate void NetListenerDelegate(NetPacket packet);

    public Listener<int, NetListenerDelegate> listeners = new Listener<int, NetListenerDelegate>();

    public delegate void NetStateDeletate(bool isConnect);

    public Listener<NetStateEvent, NetStateDeletate> netEventListener = new Listener<NetStateEvent, NetStateDeletate>();

    private NetStateDeletate currentConnectedDelegate;
    private bool isConnected = false; //不用socket==null或者socket.connected而用此标志位提前知道socket要被关闭以预防线程同步问题

    const int cBufferSize = 1024 * 1024;
    const float cSendTimeout = 5f;

    protected bool hasLogined = false;
    protected virtual int LoginCmd => -1;
    protected virtual int GatewayLoginMsgId => -1;
    private int packetSeq = 0;
    protected int UnStableCount = 0;

    public override void Init()
    {
        buffer = new ByteBuffer(cBufferSize);
        // 由于无法控制socket对缓冲区的写入过程, 所以无法进行动态扩容, 直接将初始值设置的大一些
        tempBuffer = new ByteBuffer(cBufferSize);
        sendBuffer = new ByteBuffer(cBufferSize);

        recvPool = new NetPool();
        sendPool = new NetPool();

        timeoutPacket.Clear();

        recvThread = new Thread(new ThreadStart(OnSocketReceive));
        recvThread.Name = "DIDANetmanager_Recv";
        recvThread.IsBackground = false;
        recvThread.Priority = System.Threading.ThreadPriority.Normal;
        recvThread.Start();

        sendThread = new Thread(new ThreadStart(OnSocketSend));
        sendThread.Name = "DIDANetmanager_Send";
        sendThread.IsBackground = false;
        sendThread.Priority = System.Threading.ThreadPriority.Normal;
        sendThread.Start();

        Volatile.Write(ref isConnected, false);
        hasLogined = false;
    }

    int? removeKey = -1;
    private bool clearBool = false;

    public override void DoUpdate()
    {
        if (!IsSocketValid())
            return;
        removeKey = null;
        clearBool = false;
        PluginUtilities.ProfilerBegin("NetManager.DoUpdate");
        foreach (var e in timeoutPacket.Keys)
        {
            timeoutPacket[e].remainTime -= Time.unscaledDeltaTime;
            if (timeoutPacket[e].remainTime <= 0)
            {
                if (OnProcessTimeout(e, timeoutPacket[e]))
                {
                    clearBool = true;
                    break;
                }
                else
                {
                    removeKey = e;
                    break;
                }
            }
        }

        if (clearBool)
            timeoutPacket.Clear();
        else if (removeKey != null) timeoutPacket.Remove(removeKey.Value);

        NetPacket packet = null;
        while ((packet = recvPool.GetRecvPacket()) != null)
        {
            if (LaunchConfigManager.LogEnable("NetManager"))
            {
                //LogUtils.LogWarning("Net recv, cmd =", packet.msgid, "len = ", packet.data.Length);
            }

            DispatchCmdEvent(packet);
        }

        PluginUtilities.ProfilerEnd();
    }

    public void Connect(string host, int port, NetStateDeletate callback = null)
    {
        InvokeCurrentConnected(false);
        currentConnectedDelegate = callback;

        if (IsSocketValid())
        {
            //LogUtils.LogWarning("wait for last socket close...");
            Close(false);
            // 关闭之后, 等待真的结束
            closeEvent.Reset();
            closeEvent.WaitOne(2000);
        }
        else
        {
            Close(false);
        }

        var saea = new SocketAsyncEventArgs();
        IPAddress ipAddr;

        if (Uri.CheckHostName(host) == UriHostNameType.Dns)
        {
            var entry = Dns.GetHostEntry(host);
            ipAddr = entry.AddressList.Length > 0 ? entry.AddressList[0] : null;
            ;
        }
        else
        {
            ipAddr = IPAddress.Parse(host);
        }

        socketIPPort = "host =" + host + ", ip =" + ipAddr + ":" + port;

        saea.RemoteEndPoint = new IPEndPoint(ipAddr, port);
        saea.Completed += OnConnected;

        if (ipAddr.AddressFamily == AddressFamily.InterNetworkV6)
            m_socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
        else
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        //LogUtils.LogWarning("start connecting socket,  ", socketIPPort);

        // //newSocket.SendTimeout = cSendTimeout;
        var ConnectDelay = 5000;
        m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, ConnectDelay);
        m_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        m_socket.ConnectAsync(saea);

        IEnumerator startCoroutineFunc(CoroutineObj obj)
        {
            yield return obj.Timer(ConnectDelay / 1000, false);

            ForceAbortSocket();
        }

        CoroutineManager.Stop(connectTimeoutroutine);
        connectTimeoutroutine = CoroutineManager.Start(startCoroutineFunc);
    }

    private CoroutineObj connectTimeoutroutine;

    public virtual void Send(int msg, IMessage data, int cmdId)
    {
        if (!IsSocketValid())
        {
            LogUtils.LogError("Net socket has close... send message failed, send method =", msg, cmdId);
            return;
        }

        if (!hasLogined && cmdId != LoginCmd && msg != GatewayLoginMsgId)
        {
            LogUtils.LogError("Net unlogin state ... send message failed, send method =", msg, cmdId);
            return;
        }

        sendPool.AddSendPacket(msg, data.ToByteArray(), cmdId);
    }

    public void LuaSend(int msg, byte[] data, int cmdId)
    {
        if (!IsSocketValid())
        {
            LogUtils.LogError("Net socket has close... send message failed, send method =", msg, cmdId);
            return;
        }

        if (!hasLogined && cmdId != LoginCmd && msg != GatewayLoginMsgId)
        {
            LogUtils.LogError("Net unlogin state ... send message failed, send method =", msg, cmdId);
            return;
        }

        if (cmdId != 1)
        {
            // 超时后, 阻止所有消息的发送,直到收到回复为止
            if (IsConnectUnStable())
            {
                return;
            }
        }

        sendPool.AddSendPacket(msg, data, cmdId);
    }

    protected virtual void DispatchCmdEvent(NetPacket packet)
    {
        listeners.Dispatch(packet.cmd, (a) => a(packet));
    }

    protected void InvokeCurrentConnected(bool success)
    {
        DIDAMain.Instance.RunOnMainThread(() =>
        {
            currentConnectedDelegate?.Invoke(success);
            currentConnectedDelegate = null;
        });
    }

    private void OnConnected(object sender, SocketAsyncEventArgs e)
    {
        var bOK = e.SocketError == SocketError.Success;

        if (bOK)
        {
            packetSeq = 0; // 当前NetManager包序号
            //LogUtils.LogWarning("Net socket connnect success", socketIPPort);
            Volatile.Write(ref isConnected, true);
            connectEvent.Set();
        }
        else
        {
            //LogUtils.LogWarning("Net socket connect failed", socketIPPort);

            Close(false);
        }

        CoroutineManager.Stop(connectTimeoutroutine);
        InvokeCurrentConnected(bOK);
        DIDAMain.Instance.RunOnMainThread(() =>
        {
            netEventListener.Dispatch(NetStateEvent.Connected, (a) => a(true));
        });
    }

    private void OnSocketReceive()
    {
        while (true)
        {
            int count = 0;
            tempBuffer.Clear();

            connectEvent.WaitOne();

            try
            {
                //发热修改：socket.Available <= 0 || (count = socket.Receive(tempBuffer.BBuffer)) < 0 会导致receive不会block的情况
                if ((count = m_socket.Receive(tempBuffer.BBuffer)) < 0)
                {
                    //Disconnect();
                    continue;
                }

                tempBuffer.Count = count;
                buffer.Append(tempBuffer);

                while (buffer.CanReadInt32())
                {
                    var len = buffer.ReadInt32();

                    if (buffer.Available >= len)
                    {
                        var packet = new NetPacket();
                        packet.ReadBuffer(buffer, len);
                        if (LaunchConfigManager.LogEnable("NetManager"))
                        {
                            //LogUtils.LogWarning($"Net read num = {packet.number} cmd = {packet.cmd} zip = {packet.zip} len = {len}");
                        }

                        recvPool.AddRecvPacket(packet);

                        buffer.TruncateRead();
                    }
                    else
                    {
                        if (LaunchConfigManager.LogEnable("NetManager"))
                        {
                            //   LogUtils.LogWarning($"Net read out of bound, do revert");
                        }

                        buffer.RevertLastRead();
                        break;
                    }
                }
            }
            catch (SocketException e)
            {
                LogUtils.LogError($"Socket error : {e.SocketErrorCode} {e.ToString()}");
                Close(m_socket, true);
                closeEvent.Set();
                connectEvent.Reset();
            }
            catch (ThreadAbortException e)
            {
                //忽略Thread.Abort异常
                Close(m_socket, true);
                closeEvent.Set();
                connectEvent.Reset();
            }
            catch (Exception e)
            {
                LogUtils.LogError("Socket error : ", e);
                Close(m_socket, true);
                closeEvent.Set();
                connectEvent.Reset();
            }

            //Thread.Sleep(100);
        }
    }

    private void OnSocketSend()
    {
        NetPacket packet;
        while (true)
        {
            AutoResetEvent sendEvent = sendPool.sendEvent;
            if (sendEvent.WaitOne(2000))
            {
                if (IsSocketValid() == false)
                    continue;
                try
                {
                    while (sendPool.HasPacketToSend())
                    {
                        packet = sendPool.GetSendPacket();
                        packet.remainTime = cSendTimeout;
                        lock (timeoutPacket)
                        {
                            if (!timeoutPacket.ContainsKey(packet.msgid))
                                timeoutPacket.Add(packet.msgid, packet);
                            else
                                timeoutPacket[packet.msgid] = packet;
                        }

                        sendBuffer.Clear();
                        packet.WriteBuffer(packetSeq++, sendBuffer);
                        var i = m_socket.Send(sendBuffer.BBuffer, sendBuffer.Count, SocketFlags.None);

                        if (LaunchConfigManager.LogEnable("NetManager"))
                        {
                            //LogUtils.LogWarning("Net send", packetSeq - 1, packet.msgid, packet.data.Length, i);
                        }
                    }
                }
                catch (ThreadAbortException e)
                {
                    //忽略Thread.Abort异常
                }
                catch (Exception e)
                {
                    LogUtils.LogError("Net send error:", e);
                    Close(true);
                    continue;
                }
            }
        }
    }

    //接入必须在协议接受后调用
    protected void OnCheckCmdRecv(int cmdId)
    {
        if (timeoutPacket.ContainsKey(cmdId))
            timeoutPacket.Remove(cmdId);
    }

    protected virtual bool OnProcessTimeout(int e, NetPacket packet)
    {
        return false;
    }

    protected virtual bool IsSocketValid()
    {
        if (m_socket != null && m_socket.Connected)
        {
            return true;
        }

        return false;
    }

    // 连接超时
    void ForceAbortSocket()
    {
        //LogUtils.LogWarning($"网络连接超时, 强制关闭");
        if (m_socket != null)
        {
            m_socket.Close();
        }

        m_socket = null;
        CoroutineManager.Stop(connectTimeoutroutine);
    }

    public virtual void Close(bool sendDisconnect)
    {
        Close(m_socket, sendDisconnect);
    }

    public virtual void Close(Socket sender, bool sendDisconnect)
    {
        //LogUtils.LogWarning($"Start Close Socket, sendDisconnect = {sendDisconnect}");

        if (!Volatile.Read(ref isConnected))
        {
            return;
        }

        //LogUtils.LogWarning($"Do Close Socket, sendDisconnect = {sendDisconnect}");

        Volatile.Write(ref isConnected, false);
        hasLogined = false;

        if (m_socket != null)
        {
            m_socket.Close();
        }

        m_socket = null;

        //socket关闭后清理以防死锁
        timeoutPacket.Clear();
        UnStableCount = 0;
        // sendPool.CleanAll();
        // recvPool.CleanAll();
        connectEvent.Set();
        buffer.Clear();
        Clear();

        if (sendDisconnect)
        {
            DIDAMain.Instance.RunOnMainThread(() =>
            {
                netEventListener.Dispatch(NetStateEvent.Disconnected, (d) => d(false));
            });
        }
    }

    public override void Clear()
    {
        base.Clear();
    }

    public override void DoDestroy()
    {
        try
        {
            if (recvThread != null)
                recvThread.Abort();

            if (sendThread != null)
                sendThread.Abort();
        }
        catch (System.Threading.ThreadAbortException e)
        {
        }


        Close(false);
    }

    public bool IsConnectUnStable()
    {
        return UnStableCount > 0;
    }
}