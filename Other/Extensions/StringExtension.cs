using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class StringExtension
{
    //public static string AppendArray(this string str)
    //{

    //    dynamic d;
    //    d["a"] = "";
    //}

    //public static string AppendObj(string string str)
    //{

    //}

    public static string FirstCharToUpper(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    public static string FormatFromOne(this string input, params object[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            input = input.Replace("{" + (i + 1) + "}", args[i].ToString());
        }
        return input;
    }

    public static bool IsNull(this string input)
    {
        return string.IsNullOrEmpty(input);
    }

    /// <summary>
    /// 删除开始到cutEnding的字符字段
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cutEnding"></param>
    /// <returns></returns>
    public static string CutString(this string input, string cutEnding)
    {
        return input.Substring(0, input.Length - input.IndexOf(cutEnding) - 1);
    }

    /// <summary>
    /// 替换第一个字符
    /// </summary>
    /// <param name="text"></param>
    /// <param name="search"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string ReplaceFirst(this string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }

    public static string FolderName(this string input)
    {
        var dir = Path.GetDirectoryName(input);
        return dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
    }

    public static string Localize(this string input)
    {
        if (DIDAMain.Instance.language == null) return input;
        int tHashcode = input.GetHashCode();
        string tValue = DIDAMain.Instance.language.GetText(tHashcode);
        if (!string.IsNullOrEmpty(tValue))
            return tValue;
        return input;
    }
}
