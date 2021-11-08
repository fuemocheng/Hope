using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using NetFrame;
using NetFrame.Coding;
using CmdProto;
using Google.Protobuf;

public class NetManager : Singleton<NetManager>
{
    public Socket m_socket;
    private IPEndPoint ipEndPoint;

    private SocketAsyncEventArgs connectSAEA;
    private SocketAsyncEventArgs recvSAEA;
    private SocketAsyncEventArgs sendSAEA;

    /// ������Ϣ�Ļ��� ����ճ������
    private List<byte> recvCache = new List<byte>();
    /// ������Ϣ�Ķ���
    private Queue<byte[]> writeQueue = new Queue<byte[]>();

    //���ڶ�ȡ��д��
    private bool m_bIsReading = false;
    private bool m_bIsWriting = false;

    // ճ��������, ��Ϣ������
    public LengthEncode lengthEncode;
    public LengthDecode lengthDecode;
    public ObjectEncode messageEncode;
    public ObjectDecode messageDecode;

    private NetPool sendPool;
    private NetPool recvPool;

    public delegate void NetListenerDelegate(IMessage message);
    public Listener<int, NetListenerDelegate> Listeners = new Listener<int, NetListenerDelegate>();

    private Dictionary<int, Action<IMessage>> messageCallback = new Dictionary<int, Action<IMessage>>();

    public NetManager()
    {
    }

    public void Init()
    {
        lengthEncode = LengthEncoding.Encode;
        lengthDecode = LengthEncoding.Decode;
        messageEncode = MessageEncoding.Encode;
        messageDecode = MessageEncoding.Decode;

        recvPool = new NetPool();
        sendPool = new NetPool();
    }

    public void Update()
    {
        /// DispatchPacket
        NetPacket packet = null;
        while ((packet = recvPool.GetRecvPacket()) != null)
        {
            DispatchCmdEvent(packet);
        }
    }

    #region Connect

    public void Connect(string ip, int port)
    {
        if (IsSocketValid())
        {
            Close("Close before connected");
        }

        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse(ip);
        ipEndPoint = new IPEndPoint(ipAddress, port);

        connectSAEA = new SocketAsyncEventArgs();
        connectSAEA.RemoteEndPoint = ipEndPoint;
        connectSAEA.Completed += OnConnectedCompleted;

        int connectDelay = 5000;
        m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, connectDelay);
        m_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

        m_socket.ConnectAsync(connectSAEA);

        //TODO: ��ʱ����,�������ӵ�����

    }

    private void OnConnectedCompleted(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError != SocketError.Success)
        {
            LogUtils.LogError($"Connected failed!");
            return;
        }

        Socket socket = sender as Socket;
        string strIPRemote = socket.RemoteEndPoint.ToString();
        LogUtils.LogError($"Connect {strIPRemote} succeed!");

        //recvSAEA ��ʼ��
        recvSAEA = new SocketAsyncEventArgs();
        recvSAEA.UserToken = socket;
        byte[] receiveBuffer = new byte[1024 * 4];
        recvSAEA.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
        recvSAEA.Completed += OnReceiveCompleted;
        recvSAEA.RemoteEndPoint = ipEndPoint;

        //sendSAEA ��ʼ��
        sendSAEA = new SocketAsyncEventArgs();
        sendSAEA.UserToken = socket;
        sendSAEA.Completed += OnSendCompleted;

        m_socket.ReceiveAsync(recvSAEA);
    }

    #endregion


    #region Receive

    private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.OperationAborted)
        {
            LogUtils.LogError($"OperationAborted!");
            return;
        }

        Socket socket = sender as Socket;

        //�ж�������Ϣ�����Ƿ�ɹ�
        if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
        {
            //���� recvSAEA �����е�����
            byte[] message = new byte[e.BytesTransferred];
            Buffer.BlockCopy(e.Buffer, 0, message, 0, e.BytesTransferred);

            //������Ϣ
            Receive(message);

            //�����첽������Ϣ
            socket.ReceiveAsync(recvSAEA);
        }
        else if (e.SocketError == SocketError.ConnectionReset && e.BytesTransferred == 0)
        {
            LogUtils.LogError($"Server disconnected!");
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// ������Ϣ����, recvSAEA �������ݿ����� cache ��
    /// </summary>
    private void Receive(byte[] buff)
    {
        recvCache.AddRange(buff);
        if (!m_bIsReading)
        {
            m_bIsReading = true;
            OnReceive();
        }
    }

    /// <summary>
    /// �����������ݽ��д���, �Ƚ��볤��, �ٽ�����Ϣ
    /// </summary>
    private void OnReceive()
    {
        //������Ϣ�洢����
        byte[] buff = null;

        //��ճ�����������ڵ�ʱ�򣬽���ճ������
        if (null != lengthDecode)
        {
            buff = lengthDecode(ref recvCache);

            //��Ϣδ����ȫ �˳����ݴ��� �ȴ��´���Ϣ����
            if (null == buff)
            {
                m_bIsReading = false;
                return;
            }
        }
        else
        {
            //���ô���ճ��
            //��������û������ ֱ���������ݴ��� �ȴ���Ϣ����
            if (recvCache.Count == 0)
            {
                m_bIsReading = false;
                return;
            }
            buff = recvCache.ToArray();
            recvCache.Clear();
        }

        //�����л������Ƿ���� �˷����������
        if (null == messageDecode) { throw new Exception("Message decode process is null"); }

        //������Ϣ�����л�
        NetPacket netPacket = messageDecode(buff);

        //�㲥������Ϣ
        //�Ȼ�������������Update�д�����ֹ����ʱ�������ɶ���
        recvPool.AddRecvPacket(netPacket);

        //β�ݹ� ��ֹ����Ϣ�洢������ ��������Ϣ�����û�о�������
        OnReceive();
    }

    #endregion


    #region Send

    /// <summary>
    /// ������ͻ���֪ͨ ���� �ؿͻ�����Ϣ,  �ȱ�����Ϣ, �ٱ��볤��
    /// </summary>
    public void Send(Cmd cmd, IMessage message, Action<IMessage> callback = null)
    {
        if (!IsSocketValid())
        {
            LogUtils.LogError("Socket is invalid!");
            return;
        }
        NetPacket netPacket = new NetPacket();
        netPacket.cmd = (int)cmd;
        netPacket.message = message;

        if(callback != null)
        {
            Subscribe((int)cmd, callback);
        }

        //�ȱ�����Ϣ, �ٱ��볤��
        byte[] byteArr = lengthEncode(messageEncode(netPacket));
        Write(byteArr);
    }

    public void Write(byte[] value)
    {
        writeQueue.Enqueue(value);
        if (!m_bIsWriting)
        {
            m_bIsWriting = true;
            OnWrite();
        }
    }

    private void OnWrite()
    {
        //�жϷ��Ͷ����Ƿ�����Ϣ
        if (writeQueue.Count == 0)
        {
            m_bIsWriting = false;
            return;
        }
        //ȡ����һ��������Ϣ
        byte[] buff = writeQueue.Dequeue();
        //������Ϣ�����첽����ķ������ݻ���������
        sendSAEA.SetBuffer(buff, 0, buff.Length);
        //�����첽����
        bool willRaiseEvent = m_socket.SendAsync(sendSAEA);
        //�Ƿ����
        if (!willRaiseEvent)
        {
            if (sendSAEA.SocketError != SocketError.Success)
            {
                Close(sendSAEA.SocketError.ToString());
            }
            else
            {
                //��Ϣ���ͳɹ����ص�
                OnWrite();
            }
        }
    }

    private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
    {
        LogUtils.LogError("OnSendCompleted");
    }

    #endregion


    #region Dispatch

    /// <summary>
    /// �㲥ע���¼�
    /// </summary>
    /// <param name="packet"></param>
    private void DispatchCmdEvent(NetPacket packet)
    {
        /// �㲥ע��
        Listeners.Dispatch(packet.cmd, (a) => a(packet.message));
        /// �㲥�ص�
        DispatchCmdCallback(packet);
    }

    /// <summary>
    /// �㲥�ص�
    /// </summary>
    /// <param name="packet"></param>
    protected void DispatchCmdCallback(NetPacket packet)
    {
        if (messageCallback.TryGetValue(packet.cmd, out Action<IMessage> callback))
        {
            callback?.Invoke(packet.message);
            UnSubscribe(packet.cmd, callback);
        }
    }

    /// <summary>
    /// ������Ϣ�ص��¼�
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="callback"></param>
    public void Subscribe(int cmd, Action<IMessage> callback)
    {
        if (!messageCallback.ContainsKey((int)cmd))
            messageCallback[cmd] = null;

        messageCallback[cmd] += callback;
    }

    /// <summary>
    /// ȡ��������Ϣ�ص��¼�
    /// </summary>
    /// <param name="cmd"></param>
    public void UnSubscribe(int cmd, Action<IMessage> callback)
    {
        if (messageCallback.ContainsKey((int)cmd))
            messageCallback[cmd] -= callback;
    }

    #endregion


    #region Utils

    private bool IsSocketValid()
    {
        if (m_socket != null && m_socket.Connected)
        {
            return true;
        }
        return false;
    }

    #endregion


    private void Close(string error)
    {
        if (m_socket != null)
        {
            m_socket.Close();
            m_socket = null;
            LogUtils.LogError(error);

            //��������
            recvCache.Clear();
            writeQueue.Clear();

            Listeners.Clear();
            messageCallback.Clear();

            recvPool.Clear();
            sendPool.Clear();
        }
    }
}
