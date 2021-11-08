using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XLua.LuaDLL
{
    /// <summary>
    /// 使用xLua提供的加载静态库方法AddBuildin，这个方法可以指定在lua侧指定特定库文件名在C#端的加载方法，并且限定了格式
    /// name：buildin模块的名字，require时输入的参数；
    /// initer：初始化函数，原型是这样的public delegate int lua_CSFunction(IntPtr L)，必须是静态函数，
    /// 而且带MonoPInvokeCallbackAttribute属性修饰，这个api会检查这两个条件。
    /// </summary>
    public partial class Lua
    {
        //增加lua-protobuf支持
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLuaProfobuf(System.IntPtr L)
        {
            return luaopen_pb(L);
        }

        //增加rapidjson支持
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_rapidjson(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadRapidJson(System.IntPtr L)
        {
            return luaopen_rapidjson(L);
        }

        //增加lpeg支持
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_lpeg(IntPtr luaState);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLpeg(System.IntPtr L)
        {
            return luaopen_lpeg(L);
        }

        //增加ffi支持
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_ffi(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadFFI(System.IntPtr L)
        {
            return luaopen_ffi(L);
        }

    }
}
