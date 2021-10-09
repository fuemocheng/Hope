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
    /// lua�ļ�����
    /// </summary>
    private static Dictionary<string, byte[]> m_bytesDict = new Dictionary<string, byte[]>();

    /// <summary>
    /// lua��׺
    /// </summary>
    static string _luaSuffix = "";

    static bool _useAssetBundle = false;

    public static void Init()
    {
        if (_inited) 
            return;
        _inited = true;
        
        //ʹ�� AssetBundle
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
    /// �Զ���lua���ط���
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
        //LuaPanda ����
        _luaEnv.DoString("require 'LuaFrame/LuaPanda'.start('127.0.0.1', 8818)");
#endif

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

        //���Lua��δ�ֶ��ͷŵ�LuaBase���󣨱��磺LuaTable�� LuaFunction�����Լ�����һЩ���顣
        //��Ҫ���ڵ��ã�������MonoBehaviour��Update�е��á�
        _luaEnv?.Tick();
    }


    public static void OnDestroy()
    {
        m_bytesDict.Clear();
        _luaEnv?.Dispose();
    }
}
