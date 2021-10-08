using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

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
        string newpath = $"Assets/AssetBundles/Luas/{filepath}";
        TextAsset asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(newpath);
        if(asset != null)
        {
            m_bytesDict.Add(filepath, asset.bytes);
            return asset.bytes;
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
        //_luaEnv.DoString("require 'Luas/LuaFrame/LuaPanda'.start('127.0.0.1', 8818)");

        _luaEnv.DoString("require 'LuaFrame/LuaEntry'");
    }


}
