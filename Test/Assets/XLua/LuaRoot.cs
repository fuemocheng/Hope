using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CSharpCallLua]
[LuaCallCSharp]
public static class LuaRoot
{
    //all lua behaviour shared one luaenv only!
    static LuaEnv _luaEnv = new LuaEnv();

    static LuaTable _envTable;

    static Action _start;
    static Action _update;
    static Action _destory;

    static bool _inited;

    /// <summary>
    /// lua文件缓存
    /// </summary>
    private static Dictionary<string, byte[]> m_bytesDict = new Dictionary<string, byte[]>();

    /// <summary>
    /// lua后缀
    /// </summary>
    static string _luaSuffix = "";

    static bool _useAssetBundle = false;

    public static void Init()
    {
        if (_inited) 
            return;
        _inited = true;
        
        //使用 AssetBundle
        if (_useAssetBundle)
        {
            _luaSuffix = ".lua.bytes";
        }
        else
        {
            _luaSuffix = ".lua";
        }

        _luaEnv.AddLoader(CustomLoader);

        //_luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
        //_luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        //_luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);

        StartLua();
    }

    /// <summary>
    /// 自定义lua加载方法
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private static byte[] CustomLoader(ref string filepath)
    {
        if (m_bytesDict.ContainsKey(filepath))
            return m_bytesDict[filepath];
#if UNITY_EDITOR
        string newpath = $"Assets/AssetBundles/Luas/{filepath}" + _luaSuffix;
        LuaAsset luaAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<LuaAsset>(newpath);
        if(luaAsset != null)
        {
            var data = luaAsset.GetDecodeBytes();
            m_bytesDict.Add(filepath, data);
            return data;
        }
#endif
        return null;
    }

    public static void StartLua()
    {
        _envTable = _luaEnv.NewTable();
        var meta = _luaEnv.NewTable();
        meta.Set("__index", _luaEnv.Global);
        _envTable.SetMetaTable(meta);

        meta.Dispose();

#if UNITY_EDITOR
        //LuaPanda 调试
        _luaEnv.DoString("require 'LuaFrame/LuaPanda'.start('127.0.0.1', 8818)");
        _luaEnv.DoString("UNITY_EDITOR = true");
#endif
        //lua总入口
        _luaEnv.DoString("require 'LuaFrame/LuaEntry'");

        _envTable.Get("CSStart", out _start);
        _envTable.Get("CSUpdate", out _update);

        _start?.Invoke();
    }


    public static void Update()
    {
        if (!_inited)
            return;

        _update?.Invoke();

        //清除Lua的未手动释放的LuaBase对象（比如：LuaTable， LuaFunction），以及其它一些事情。
        //需要定期调用，比如在MonoBehaviour的Update中调用。
        _luaEnv?.Tick();
    }

    public static void Dispose()
    {
        m_bytesDict.Clear();
        _luaEnv = new LuaEnv();
        _inited = false;
    }

    public static void OnDestroy()
    {
        m_bytesDict.Clear();
        _luaEnv?.Dispose();
        _inited = false;
    }
}



#if UNITY_EDITOR
public class LuaFastProcessor : AssetPostprocessor
{
    static string assetBundlePath = "Assets/AssetBundles/";
    static List<string> luaFiles = new List<string>();
    public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (Application.isPlaying)
        {
            //luaFiles.Clear();
            //for (int i = 0; i < importedAsset.Length; i++)
            //{
            //    bool isLuaFile = importedAsset[i].EndsWith(".lua");
            //    if (isLuaFile)
            //    {
            //        string luaPath = importedAsset[i];

            //        luaPath = luaPath.Substring(assetBundlePath.Length, luaPath.Length - assetBundlePath.Length);
            //        luaPath = luaPath.Replace(".lua", "");
            //        luaFiles.Add(luaPath);

            //    }
            //}
            //if (luaFiles.Count > 0)
            //{
            //    EditorUtility.DisplayProgressBar("HotUpdateLua", "", 0);
            //    try
            //    {
            //        LuaRoot.HotUpdate(luaFiles);
            //        Debug.Log("LuaHotUpdate:" + string.Join(",", luaFiles.ToArray()));
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.LogException(e);
            //    }

            //    luaFiles.Clear();
            //    EditorUtility.ClearProgressBar();
            //}
        }
    }
}
#endif