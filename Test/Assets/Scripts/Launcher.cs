using GameProto;
using Google.Protobuf;
using NetFrame.Coding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoSingleton<Launcher>
{
    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "MyGameTag";
    private CustomLogHandler m_CustomLogHandler;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        /// Log初始化
        m_CustomLogHandler = new CustomLogHandler();
        /// 网络初始化
        NetManager.Instance.Init();
        /// Lua初始化
        LuaRoot.Init();
    }

    void Update()
    {
        NetManager.Instance.Update();
        LuaRoot.Update();
    }

    void OnDestroy()
    {
        LuaRoot.OnDestroy();
    }

    public void Test()
    {
        //var time1 = Time.realtimeSinceStartup;
        //LuaUtility.Instance.CallLuaFunc("OnClickTest");
        //var time2 = Time.realtimeSinceStartup;
        //Debug.LogError(string.Format("{0:F6}", time2 - time1));
        
    }

    public void Connect()
    {
        LogUtils.LogError("Test Connect");
        NetManager.Instance.Connect("127.0.0.1", 8001);

        //NetManager.Instance.Listeners.Add((int)CmdProto.Cmd.Login, OnLoginAck);
    }

    private void OnLoginAck(IMessage message)
    {
        var msg = message as LoginAck;
        bool createRole = msg.CreateRole;
        LogUtils.LogError("createRole:", createRole);
    }

    //private void OnLoginAck(IMessage packet)
    //{
    //    var msg = packet.GetMessge<GameProto.LoginAck>();
    //    bool createRole = msg.CreateRole;

    //    LogUtils.LogError("createRole:" , createRole);
    //}

    public void Login()
    {
        GameProto.LoginReq loginReq = new GameProto.LoginReq
        {
            Token = "client_1",
            Relogin = false,
            GameId = 101010101,
            ChannelId = 202020201,
        };
        NetManager.Instance.Send(CmdProto.Cmd.Login, loginReq, OnLoginAck);
    }
}
