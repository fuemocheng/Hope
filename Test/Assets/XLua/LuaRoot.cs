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

    public static void Init()
    {
        if (_inited) 
            return;
        _inited = true;
        _envTable = _luaEnv.NewTable();

    }



}
