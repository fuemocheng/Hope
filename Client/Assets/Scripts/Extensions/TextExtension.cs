using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO 需要释放
public static class TextExtension
{
    private static Dictionary<Text, string> textDic = new Dictionary<Text, string>();

    /// <summary>
    /// 对Text格式化并保存原始字符串
    /// </summary>
    public static void FormatText(this Text text, params object[] args)
    {

        //保存格式化字符串以复用否则下次无法再次格式化
        if (!textDic.ContainsKey(text))
        {
            textDic.Add(text, text.text);
        }

        text.text = string.Format(textDic[text], args);
    }

    /// <summary>
    ///格式化从{1}开始而不是{0}
    /// </summary>
    public static void FormatTextFromOne(this Text text, params object[] args)
    {

        //保存格式化字符串以复用否则下次无法再次格式化
        if (!textDic.ContainsKey(text))
        {
            textDic.Add(text, text.text);
        }

        text.text = textDic[text].FormatFromOne(args);
    }
}