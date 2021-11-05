using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class LogUtils
{
    //�ַ�������
    private static StringBuilder s_stringBuilder = new StringBuilder(1024 * 4);
    //�����ַ���ƴ����ɵ�GC
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
    /// ��Ϣ��־
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void Log(params object[] args)
    {
        UnityEngine.Debug.Log(BuildString(args));
    }
    
    /// <summary>
    /// ������־
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogWarning(params object[] args)
    {
        UnityEngine.Debug.LogWarning(BuildString(args));
    }

    /// <summary>
    /// ������־
    /// </summary>
    [Conditional("DEBUG")]
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public static void LogError(params object[] args)
    {
        UnityEngine.Debug.LogError(BuildString(args));
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
