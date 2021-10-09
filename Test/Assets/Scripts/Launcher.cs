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
}
