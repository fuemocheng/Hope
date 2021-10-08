using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class LogUtils
{
    /// <summary>
    /// 信息日志
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void Log(params object[] args)
    {
        UnityEngine.Debug.Log(args);
    }
    
    /// <summary>
    /// 警告日志
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogWarning(params object[] args)
    {
        UnityEngine.Debug.LogWarning(args);
    }

    /// <summary>
    /// 错误日志
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogError(params object[] args)
    {
        UnityEngine.Debug.LogError(args);
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
