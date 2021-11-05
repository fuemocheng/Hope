using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//客户端主机模式，基础服务器
public class NetServerManager : NetManager
{
    protected Thread acceptThread = null;
    protected Dictionary<string, NetServerConnection> servers;
    protected List<string> hostedIpList;
    protected List<string> connectedIpList;
    protected NetServerConnection selfData;

    public new static NetServerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NetServerManager();
                _instance.Init();
            }
            else if (!(_instance is NetServerManager))
            {
                var last = _instance;
                _instance = new NetServerManager();
                _instance.Init();
                (_instance as NetServerManager).InheritFrom(last);

                last.DoDestroy();
            }
            return _instance as NetServerManager;
        }
    }

    public override void Init()
    {
        servers = new Dictionary<string, NetServerConnection>();
        hostedIpList = new List<string>();
        connectedIpList = new List<string>();
    }

    public override void DoUpdate()
    {
        PluginUtilities.ProfilerBegin("NetServerManager.DoUpdate");
        foreach (var e in servers)
        {
            if (!e.Value.IsSocketValid())
            {
                OnConnectionChange(e.Value, false);
                servers.Remove(e.Key);
                break;
            }
        }
        PluginUtilities.ProfilerEnd();
    }

    public void InheritFrom(NetManager super)
    {
        //TODO
        //foreach (var e in super.listeners)
        //{
        //    listeners.Add(e.Key, e.Value);
        //}
    }

    public void Host(string ip, int port, List<string> allIp = null)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var iep = new IPEndPoint(IPAddress.Parse(ip), port);
        m_socket.Bind(iep);
        m_socket.Listen(0);

        hostedIpList.Clear();
        if (allIp != null)
            hostedIpList.AddRange(allIp);

        connectedIpList.Clear();

        acceptThread = new Thread(new ThreadStart(OnSocketAccept));
        acceptThread.IsBackground = false;
        acceptThread.Priority = ThreadPriority.Normal;
        acceptThread.Start();
    }

    public override void Send(int msg, IMessage data, int cmdId)
    {
        NetPacket packet = new NetPacket();
        packet.number = 0;
        packet.cmd = msg;
        packet.data = data.ToByteArray();

        selfData.receivePacketList.Add(packet);
    }

    public virtual void Notify<T>(T data)
    {
        NetPacket packet = new NetPacket();
        packet.number = 0;
        packet.cmd = AbstractProto<T>.protoCmd;
        packet.data = AbstractProto<T>.encoder(data);

        foreach (var server in servers.Values)
        {
            LogUtils.Log("Net server notify msg, len =", packet.data.Length);

            if (server == selfData)
            {
                //TODO
                //if (listeners.ContainsKey(packet.cmd))
                //    listeners[packet.cmd](packet);
            }
            else
                server.Send(packet);
        }
    }

    protected void OnSocketAccept()
    {
        if (!IsSocketValid())
            return;

        while (true)
        {
            if (!IsSocketValid())
                return;

            var svr = m_socket.Accept();
            var ipEndPoint = svr.RemoteEndPoint as IPEndPoint;
            var ipAddress = ipEndPoint.Address.ToString();

            if (servers.ContainsKey(ipAddress))
            {
                var s = servers[ipAddress];
                s.Close();
                servers.Remove(ipAddress);
            }

            if (!hostedIpList.Contains(ipAddress))
            {
                svr.Close();
                continue;
            }

            var server = new NetServerConnection(svr);
            servers.Add(ipAddress, server);
        }
    }

    protected virtual void OnConnectionChange(NetServerConnection conn, bool isConnect)
    {

    }

    protected override bool IsSocketValid()
    {
        if (m_socket == null)
        {
            //Close();
            return false;
        }
        return true;
    }
}

