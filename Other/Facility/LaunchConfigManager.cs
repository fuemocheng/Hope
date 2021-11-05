using LitJson;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LaunchConfigManager : InstanceBase<LaunchConfigManager>
{
    public const string CFG_PATH = "cfg.json";
    public const string API_PATH = "api.json";
    public const string BATTLE_LOG_PATH = "battle_log.json";
    public const string BATTLE_LOG_LAST_PATH = "battle_log_last.json";
    public const string ORIGIN_CFG_PATH = "Plugins/DIDA/cfg.json";

    public static LaunchConfig Config { get; private set; }

    public override void Init()
    {
        var data = string.Empty;

        if (Application.platform == RuntimePlatform.Android)
        {
            //LogUtils.Log("loading launch cfg at android... path =", CFG_PATH);

            var url = new System.Uri(CFG_PATH.DataPath());
            UnityWebRequest.ClearCookieCache();
            UnityWebRequest reader = UnityWebRequest.Get(url);
            reader.SendWebRequest();

            while (!reader.isDone)
            {
                //.Log("loading launch cfg...");
            }

            data = reader.downloadHandler.text;

            //LogUtils.Log("loading launch cfg at android... data =", data);

            //JsonData jsond = JsonMapper.ToObject(File.ReadAllText(API_PATH));
            //if(jsond.Count > 0 && jsond[0].Count > 0)
            //{
            //    Config.APPID = Convert.ToInt32(jsond[0]["appid"]);
            //    Debug.Log(Config.APPID);
            //}


        }
        else
        {
            var file = new FileInfo(CFG_PATH.DataPath());
            if (file.Exists)
            {
                var fs = file.OpenText();
                data = fs.ReadToEnd();
                fs.Close();
            }
        }

        if (!string.IsNullOrEmpty(data))
        {
            //LogUtils.Log(data);
            Config = JsonMapper.ToObject<LaunchConfig>(data);
        }
        else
        {
            //LogUtils.Log("load launch cfg failed...");
            Config = new LaunchConfig();
        }


        if (Application.platform == RuntimePlatform.Android)
        {
            //LogUtils.Log("loading launch cfg at android... path =", API_PATH);

            var url = new System.Uri(API_PATH.DataPath());
            UnityWebRequest.ClearCookieCache();
            UnityWebRequest reader = UnityWebRequest.Get(url);
            reader.SendWebRequest();

            while (!reader.isDone)
            {
                //LogUtils.Log("loading launch cfg...");
            }

            data = reader.downloadHandler.text;

            //LogUtils.Log("loading launch cfg at android... data =", data);

        }
        else
        {
            var file = new FileInfo(API_PATH.DataPath());
            if (file.Exists)
            {
                var fs = file.OpenText();
                data = fs.ReadToEnd();
                fs.Close();
            }
        }

        if (!string.IsNullOrEmpty(data))
        {
#if BRANCH_TAPTAP
            Config.APPID = 12;
            Debug.Log("Config.APPID = " + Config.APPID);
#else
            //LogUtils.Log(data);
            JsonData deJson = LitJson.JsonMapper.ToObject(data);
            if (deJson.Keys.Contains("appid"))
            {
                Config.APPID = int.Parse(deJson["appid"].ToString());
                //Debug.Log("Config.APPID = " + Config.APPID);
            }
            if (deJson.Keys.Contains("zoneid"))
            {
                Config.ZONEID = int.Parse(deJson["zoneid"].ToString());
                //Debug.Log("Config.APPID = " + Config.ZONEID);
            }
#endif
        }
        else
        {
            //LogUtils.Log("load launch appid failed...");
        }
    }

    public override void DoUpdate()
    {


    }

    public override void DoDestroy()
    {

    }

    public static bool m_bRefreshBattaleLogWindow = false;
    public static void SaveBattleLog(string logStr)
    {
        var curFile = new FileInfo(BATTLE_LOG_PATH);
        if (curFile.Exists)
        {
            using (var curSW = curFile.OpenText())
            {
                using (var lastSw = new StreamWriter(BATTLE_LOG_LAST_PATH, false))
                    lastSw.Write(curSW.ReadToEnd());
            }
        }

        using (var curSW = new StreamWriter(BATTLE_LOG_PATH, false))
        {
            curSW.Write(logStr);
        }

        m_bRefreshBattaleLogWindow = true;
    }

    public static bool LogEnable(string str)
    {
        return Config.LogEnable.Contains(str);
    }
}

public class LaunchConfig
{
    public string[] LogEnable;
    public int ServerIndex = 0;
    public bool CatchCrashException = true;
    public bool SaveBattleUpdateLog = true;
    public bool SkipLogin = false;
    public bool ForceCamp = true;
    public int FrameRate = 30;
    public bool EnableHotfix = false;
    public int APPID = 1;
    public int ZONEID = 1;
}