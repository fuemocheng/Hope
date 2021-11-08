using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XLua.LuaDLL
{
    /// <summary>
    /// ʹ��xLua�ṩ�ļ��ؾ�̬�ⷽ��AddBuildin�������������ָ����lua��ָ���ض����ļ�����C#�˵ļ��ط����������޶��˸�ʽ
    /// name��buildinģ������֣�requireʱ����Ĳ�����
    /// initer����ʼ��������ԭ����������public delegate int lua_CSFunction(IntPtr L)�������Ǿ�̬������
    /// ���Ҵ�MonoPInvokeCallbackAttribute�������Σ����api����������������
    /// </summary>
    public partial class Lua
    {
        //����lua-protobuf֧��
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLuaProfobuf(System.IntPtr L)
        {
            return luaopen_pb(L);
        }

        //����rapidjson֧��
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_rapidjson(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadRapidJson(System.IntPtr L)
        {
            return luaopen_rapidjson(L);
        }

        //����lpeg֧��
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_lpeg(IntPtr luaState);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLpeg(System.IntPtr L)
        {
            return luaopen_lpeg(L);
        }

        //����ffi֧��
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_ffi(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadFFI(System.IntPtr L)
        {
            return luaopen_ffi(L);
        }

    }
}
