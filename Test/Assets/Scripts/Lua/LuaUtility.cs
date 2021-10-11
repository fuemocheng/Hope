using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaUtility : Singleton<LuaUtility>
{
    static LuaEnv s_luaEnv = null;
    static LuaTable s_globalTable = null;

    //缓存，防止每次都去取
    static Dictionary<int, LuaFunction> s_cachedLuaFunction = new Dictionary<int, LuaFunction>(50);

    static public LuaEnv luaEnv
    {
        get { return s_luaEnv; }
        set { s_luaEnv = value; }
    }

    static public LuaTable globalTable
    {
        get { return s_globalTable; }
        set { s_globalTable = value; }
    }


    public void CallLuaFunc(string funcName)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            tmpFunc.Action();
        }
    }

    public void CallLuaFunc<T1>(string funcName, T1 para1)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            tmpFunc.Action<T1>(para1);
        }
    }

    public void CallLuaFunc<T1, T2>(string funcName, T1 para1, T2 para2)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if(tmpFunc != null)
        {
            tmpFunc.Action<T1, T2>(para1, para2);
        }
    }

    public void CallLuaFunc<T1, T2, T3>(string funcName, T1 para1, T2 para2, T3 para3)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            tmpFunc.Action<T1, T2, T3>(para1, para2, para3);
        }
    }

    public void CallLuaFunc<T1, T2, T3, T4>(string funcName, T1 para1, T2 para2, T3 para3, T4 para4)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            tmpFunc.Action<T1, T2, T3, T4>(para1, para2, para3, para4);
        }
    }

    public TResult CallLuaFunc<TResult>(string funcName)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if(tmpFunc != null)
        {
            return tmpFunc.Func<TResult>();
        }
        return default(TResult);
    }

    public TResult CallLuaFunc<T1, TResult>(string funcName, T1 para1)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            return tmpFunc.Func<T1,TResult>(para1);
        }
        return default(TResult);
    }

    public TResult CallLuaFunc<T1, T2, TResult>(string funcName, T1 para1, T2 para2)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            return tmpFunc.Func<T1,T2, TResult>(para1, para2);
        }
        return default(TResult);
    }

    public TResult CallLuaFunc<T1, T2, T3, TResult>(string funcName, T1 para1, T2 para2, T3 para3)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            return tmpFunc.Func<T1, T2, T3, TResult>(para1, para2, para3);
        }
        return default(TResult);
    }

    public TResult CallLuaFunc<T1, T2, T3, T4, TResult>(string funcName, T1 para1, T2 para2, T3 para3, T4 para4)
    {
        LuaFunction tmpFunc = GetLuaFunction(funcName);
        if (tmpFunc != null)
        {
            return tmpFunc.Func<T1, T2, T3, T4, TResult>(para1, para2, para3, para4);
        }
        return default(TResult);
    }

    static internal LuaFunction GetLuaFunction(string strFuncName)
    {
        LuaFunction luaFunc = null;
        if (string.IsNullOrEmpty(strFuncName))
            return luaFunc;

        //缓存
        int hashCode = strFuncName.GetHashCode();
        s_cachedLuaFunction.TryGetValue(hashCode, out luaFunc);
        if (luaFunc != null)
            return luaFunc;

        string strTableName = string.Empty;
        int index = strFuncName.IndexOf('.');
        if (index > 0)
        {
            strTableName = strFuncName.Substring(0, index);
            strFuncName = strFuncName.Substring(index + 1);
        }

        //如果热更，global table 在这里更换
        LuaTable global = globalTable;

        if (string.IsNullOrEmpty(strTableName))
        {
            luaFunc = global.Get<LuaFunction>(strFuncName);
        }
        else
        {
            LuaTable tTable = global.Get<LuaTable>(strTableName);
            if (tTable != null)
            {
                luaFunc = tTable.Get<LuaFunction>(strFuncName);
            }
        }

        if (luaFunc != null)
        {
            s_cachedLuaFunction.Add(hashCode, luaFunc);
        }

        return luaFunc;
    }


    public void LuaDoString(string strLua)
    {
        luaEnv?.DoString(strLua);
    }

    public object[] LuaDoStringWithResultValue(string strLua)
    {
        return luaEnv != null ? luaEnv?.DoString(strLua) : null;
    }

    public int LuaMemory()
    {
        return luaEnv != null ? luaEnv.Memroy : 0;
    }

    public List<(int, int, string)> LuaObjectMap()
    {
        return luaEnv != null ? luaEnv.translator.RetriveLuaObjectMap() : null;
    }
}
