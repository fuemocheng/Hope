using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class LogUtils
{
    //字符缓存区
    private static StringBuilder s_stringBuilder = new StringBuilder(1024 * 4);
    //减少字符串拼接造成的GC
    public static string BuildString(params object[] args)
    {
        lock (s_stringBuilder)
        {
            s_stringBuilder.Length = 0;
            for (int i = 0; i < args.Length; i++)
            {
                s_stringBuilder.Append(args[i]);
                s_stringBuilder.Append(" ");
            }
            return s_stringBuilder.ToString();
        }
    }

    /// <summary>
    /// 信息日志
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void Log(params object[] args)
    {
        UnityEngine.Debug.Log(BuildString(args));
    }
    
    /// <summary>
    /// 警告日志
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogWarning(params object[] args)
    {
        UnityEngine.Debug.LogWarning(BuildString(args));
    }

    /// <summary>
    /// 错误日志
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogError(params object[] args)
    {
        UnityEngine.Debug.LogError(BuildString(args));
    }

    /// <summary>
    /// 异常日志
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }
}
