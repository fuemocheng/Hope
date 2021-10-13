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
        var time1 = Time.realtimeSinceStartup;
        LuaUtility.Instance.CallLuaFunc("OnClickTest");
        var time2 = Time.realtimeSinceStartup;
        Debug.LogError(string.Format("{0:F6}", time2 - time1));
    }
}
