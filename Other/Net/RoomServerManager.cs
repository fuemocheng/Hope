using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

//客户端主机模式，房间服务器
public class RoomServerManager : NetServerManager
{
    private bool hasResend = false;
    private List<string> ipList = new List<string>();
    private Thread httpThread;
    private HttpListener httpListener;

    public new static RoomServerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RoomServerManager();
                _instance.Init();
            }
            else if (!(_instance is NetServerManager))
            {
                var last = _instance;
                _instance = new RoomServerManager();
                _instance.Init();
                (_instance as NetServerManager).InheritFrom(last);

                last.DoDestroy();
            }
            return _instance as RoomServerManager;
        }
    }

    public override void Init()
    {
        base.Init();

        httpListener = new HttpListener();
        httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        httpListener.Prefixes.Add("http://192.168.133.97/");
        httpListener.Start();

        httpThread = new Thread(new ThreadStart(HttpReceive));
        httpThread.Start();
    }

    public override void DoUpdate()
    {
        PluginUtilities.ProfilerBegin("RoomServerManager.DoUpdate");
        base.DoUpdate();

        if (!hasResend)
        {
            hasResend = true;

            ipList.Clear();
            ipList.Add(m_socket.GetAddressAndPort());
            foreach (var v in servers.Keys)
            {
                ipList.Add(v);
            }

            var roomNotify = new RoomMessageNotify();
            roomNotify.ipList = ipList.ToArray();

            Notify(roomNotify);
        }
        PluginUtilities.ProfilerEnd();
    }

    protected void HttpReceive()
    {
        while (true)
        {
            var context = httpListener.GetContext();
            context.Response.StatusCode = 200;

            using (var writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.WriteLine(servers.Count + 1);
            }
        }
    }

    protected override void OnConnectionChange(NetServerConnection conn, bool isConnect)
    {
        hasResend = false;
    }
}
