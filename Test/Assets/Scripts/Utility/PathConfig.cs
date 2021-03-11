using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PathConfig
{
    //最大bundle加载数量
    public static readonly int MAXBUNDLECOUNT = 5;

    public static readonly string bundleSuffix = ".unity3d";
    //保存打包的资源名与对应的MD5码
    public static readonly string version_file = "version.txt";
    public static readonly string localUrl = Application.persistentDataPath;
    //服务器下载资源地址
    public static readonly string serverUrl = "";
    public static readonly string bulidAssetPath = Application.streamingAssetsPath;

    //当前程序版本号(默认从1.0开始)
    public static string ProductVersion = "Asset_1.0";

    public static string GetFileHeader
    {
        get 
        {
#if UNITY_EDITOR
            return "file:///";
#elif UNITY_IOS
            return "";
#elif UNITYSTANDALONE_OSX
            return "";
#elif UNITY_ANDROID
            return "jar:file://";
#else
            return "file:///";
#endif
        }
    }

    public static string GetManifestFileName()
    {
        var version = ProductVersion;
#if UNITY_EDITOR
        return "Others/" + version;
#elif UNITY_IOS
        return "IOS/" + version;
#elif UNITY_STANDALONE_OSX
        return "MacOS/" + version;
#elif UNITY_ANDROID
        return "Android" + version;
#else
        return "Others/" + version;
#endif
    }

#if UNITY_EDITOR
    public static string GetBuildTargetPath(BuildTarget buildTarget)
    {
        var version = ProductVersion;
        switch (buildTarget)
        {
            case BuildTarget.iOS:
                return "IOS/" + version;
            case BuildTarget.StandaloneOSX:
                return "MacOS/" + version;
            case BuildTarget.Android:
                return "Android/" + version;
            default:
                return "Others/" + version;
        }
    }
#endif

    //检查url，如果末尾有'/'不处理，无则添加
    public static string CheckUrl(string url)
    {
        return url.Replace('\\', '/').TrimEnd('/') + '/';
    }

    //生成MD5值
    public static string MD5File(string file)
    {
        try
        {
            return MD5Checker.Check(file);
        }
        catch(System.Exception ex)
        {
            Debug.LogError(ex.Message);
            return string.Empty;
        }
    }
}
