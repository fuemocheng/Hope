using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XLua.LuaDLL
{
    /// <summary>
    /// 使用xLua提供的加载静态库方法AddBuildin，这个方法可以指定在lua侧指定特定库文件名在C#端的加载方法，并且限定了格式
    /// public void AddBuildin(string name, LuaCSFunction initer)
    /// name：buildin模块的名字，require时输入的参数；
    /// initer：初始化函数，原型是这样的public delegate int lua_CSFunction(IntPtr L)，必须是静态函数，
    /// 而且带MonoPInvokeCallbackAttribute属性修饰，这个api会检查这两个条件。
    /// </summary>
    public partial class Lua
    {
        #region Calling convention , MonoPInvokeCallback
        // 调用约定(Calling convention)：决定函数参数传送时入栈和出栈的顺序，由调用者还是被调用者把参数弹出栈，以及编译器用来识别函数名字的修饰约定。

        // CallingConvention.Cdecl --- C调用约定（即用__cdecl关键字说明）按从右至左的顺序压参数入栈，由调用者把参数弹出栈。
        // 对于传送参数的内存栈是由调用者来维护的（正因为如此，实现可变参数的函数只能使用该调用约定）。
        // 另外，在函数名修饰约定方面也有所不同。
        // _cdecl是C和C＋＋程序的缺省调用方式。每一个调用它的函数都包含清空堆栈的代码，所以产生的可执行文件大小会比调用_stdcall函数的大。
        // 函数采用从右到左的压栈方式。VC将函数编译后会在函数名前面加上下划线前缀。是MFC缺省调用约定。

        // MonoPInvokeCallbackAttribute 用来标记这个方法是由C或者C++来调用的
        // C#使用MonoPInvokeCallback，让C直接回调C#函数
        #endregion


        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLuaProfobuf(System.IntPtr L)
        {
            return luaopen_pb(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_rapidjson(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadRapidJson(System.IntPtr L)
        {
            return luaopen_rapidjson(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_lpeg(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLpeg(System.IntPtr L)
        {
            return luaopen_lpeg(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_ffi(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadFFI(System.IntPtr L)
        {
            return luaopen_ffi(L);
        }
    }
}