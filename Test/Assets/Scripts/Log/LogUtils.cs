using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class LogUtils
{
    /// <summary>
    /// ��Ϣ��־
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void Log(params object[] args)
    {
        UnityEngine.Debug.Log(args);
    }
    
    /// <summary>
    /// ������־
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogWarning(params object[] args)
    {
        UnityEngine.Debug.LogWarning(args);
    }

    /// <summary>
    /// ������־
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogError(params object[] args)
    {
        UnityEngine.Debug.LogError(args);
    }

    /// <summary>
    /// �쳣��־
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }
}
