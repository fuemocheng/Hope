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
        m_CustomLogHandler = new CustomLogHandler();
        LuaRoot.Init();
    }

    void Update()
    {
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
    }

    public void Login()
    {
        GameProto.LoginReq loginReq = new GameProto.LoginReq
        {
            Token = "client_1",
            Relogin = false,
            GameId = 101010101,
            ChannelId = 202020201,
        };
        NetManager.Instance.Send(CmdProto.Cmd.Login, loginReq);
    }
}
