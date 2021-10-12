using System.IO;
using UnityEngine;

public static class PathExtension
{
    public static string DataPath(this string input)
    {
        return Application.streamingAssetsPath + "/" + input;
    }

    public static string AssetPath(this string input)
    {
        return "Assets/" + input;
    }

    //获取文件基于Assets的相对路径
    public static string RelativeAssetsPath(this string input)
    {
        input = Path.GetFullPath(input).Replace('\\', '/');
        return "Assets" + Path.GetFullPath(input).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }




}
