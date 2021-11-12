using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class ResourceManager : Singleton<ResourceManager>
{
    private static string GetPath(string path)
    {
        if (path[0] == '/') { path = path.Substring(1); }

        if (File.Exists(path)) { return path; }

        string newpath = $"Assets/AssetBundles/{path}";

        if (File.Exists(newpath)) { return newpath; }

        newpath = $"Assets/{path}";

        if (File.Exists(newpath)) { return newpath; }

        return path;
    }

    private static string GetAssetFullPath(string assetPath, Type assetType, string assetPathSuffix = "")
    {

        if (!assetPathSuffix.IsNull() && !assetPath.EndsWith(assetPathSuffix))
        {
            assetPath += assetPathSuffix;
        }

        assetPath = GetPath(assetPath);

        return assetPath;
    }

    public UnityEngine.Object Load(string path, Type type, string suffix = "")
    {
        if (path.IsNull()) return default;

        UnityEngine.Object assetResult = default;

        string assetPath = GetAssetFullPath(path, type, suffix);

        assetResult = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, type);

        return assetResult;
    }
}
