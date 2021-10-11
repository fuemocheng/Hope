using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 暂时没用
/// </summary>
public abstract class ILuaUtility
{
    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用
    /// </summary>
    /// <example>
    /// 调用Lua中test函数：CallLuaFunc("test")
    /// 调用Lua中全局表中test函数： CallLuaFunc("globalTable.test")
    /// </example>
    /// <param name="funcName">Lua函数名</param>
    /// <returns>void</returns>
    public virtual void CallLuaFunc(string funcName) { }

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用
    /// </summary>
    /// <example>
    /// 调用Lua中test函数：CallLuaFunc<int>("test", 1)
    /// 调用Lua中全局表中test函数： CallLuaFunc<float>("globalTable.test", 1.0f)
    /// </example>
    /// <typeparam name="T1"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <param name="para1"></param>
    /// <returns>void</returns>
    public virtual void CallLuaFunc<T1>(string funcName, T1 para1) { }

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用
    /// </summary>
    /// <example>
    /// 调用Lua中test函数：CallLuaFunc<int, int>("test", 1, 1)
    /// 调用Lua中全局表中test函数： CallLuaFunc<float, float>("globalTable.test", 1.0f, 2.0f)
    /// </example>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <param name="para1"></param>
    /// <param name="para2"></param>
    /// <returns>void</returns>
    public virtual void CallLuaFunc<T1, T2>(string funcName, T1 para1, T2 para2) { }

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用
    /// </summary>
    /// <example>
    /// 调用Lua中test函数：CallLuaFunc<int, int, int>("test", 1, 1, 1)
    /// 调用Lua中全局表中test函数： CallLuaFunc<float, float, int>("globalTable.test", 1.0f, 2.0f, 5)
    /// </example>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <param name="para1"></param>
    /// <param name="para2"></param>
    /// <param name="para3"></param>
    /// <returns>void</returns>
    public virtual void CallLuaFunc<T1, T2, T3>(string funcName, T1 para1, T2 para2, T3 para3) { }

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用
    /// </summary>
    /// <example>
    /// 调用Lua中test函数：CallLuaFunc<int, int, int, int>("test", 1, 1, 1, 1)
    /// 调用Lua中全局表中test函数： CallLuaFunc<float, float, int, int>("globalTable.test", 1.0f, 2.0f, 5, 1)
    /// </example>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <param name="para1"></param>
    /// <param name="para2"></param>
    /// <param name="para3"></param>
    /// <param name="para4"></param>
    /// <returns>void</returns>
    public virtual void CallLuaFunc<T1, T2, T3, T4>(string funcName, T1 para1, T2 para2, T3 para3, T4 para4) { }

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用,一个返回值
    /// </summary>
    /// <example>
    /// 调用Lua中test函数：int result = CallLuaFuncEx<int>("test")
    /// 调用Lua中全局表中test函数： float result = CallLuaFuncEx<float>("globalTable.test")
    /// </example>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <returns>TResult</returns>
    public virtual TResult CallLuaFunc<TResult>(string funcName) => default;

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用,一个返回值
    /// </summary>
    /// <example>
    /// 调用Lua中test函数：int result = CallLuaFuncEx<int, int, int>("test", 1, 1)
    /// 调用Lua中全局表中test函数： float result = CallLuaFuncEx<int, int, float>("globalTable.test", 1, 1)
    /// </example>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <param name="para1"></param>
    /// <returns>TResult</returns>
    public virtual TResult CallLuaFunc<T1, TResult>(string funcName, T1 para1) => default;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="para1"></param>
    /// <param name="para2"></param>
    /// <returns></returns>
    public virtual TResult CallLuaFunc<T1, T2, TResult>(string funcName, T1 para1, T2 para2) => default;

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用,一个返回值
    /// </summary>
    ///  <example>
    /// 调用Lua中test函数：int result = CallLuaFuncEx<int, int, int, int>("test", 1, 1, 1)
    /// 调用Lua中全局表中test函数： float result = CallLuaFuncEx<int, int, int, float>("globalTable.test", 1, 1, 1)
    /// </example>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <param name="para1"></param>
    /// <param name="para2"></param>
    /// <param name="para3"></param>
    /// <returns>TResult</returns>
    public virtual TResult CallLuaFunc<T1, T2, T3, TResult>(string funcName, T1 para1, T2 para2, T3 para3) => default;

    /// <summary>
    /// 调用LUA函数，支持全局函数调用，支持全局表格中函数调用,一个返回值
    /// </summary>
    ///  <example>
    /// 调用Lua中test函数：int result = CallLuaFuncEx<int, int, int, int, int>("test", 1, 1, 1, 1)
    /// 调用Lua中全局表中test函数： float result = CallLuaFuncEx<int, int, int, int, float>("globalTable.test", 1, 1, 1, 1)
    /// </example>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="funcName">Lua函数名</param>
    /// <param name="para1"></param>
    /// <param name="para2"></param>
    /// <param name="para3"></param>
    /// <param name="para4"></param>
    /// <returns>TResult</returns>
    public virtual TResult CallLuaFunc<T1, T2, T3, T4, TResult>(string funcName, T1 para1, T2 para2, T3 para3, T4 para4) => default;

    /// <summary>
    /// 执行Lua语句
    /// </summary>
    /// <param name="strLua"></param>
    public virtual void LuaDoString(string strLua) { }

    /// <summary>
    /// 执行带有返回值的Lua语句
    /// </summary>
    /// <param name="strLua"></param>
    /// <returns>object[]</returns>
    public virtual object[] LuaDoStringWithResultValue(string strLua) => default;

    /// <summary>
    /// lua 内存占用
    /// </summary>
    /// <returns></returns>
    public virtual int LuaMemory() => default;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual List<(int, int, string)> LuaObjectMap() => default;
}
