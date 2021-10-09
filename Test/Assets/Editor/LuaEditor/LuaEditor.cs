using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

public class LuaEditor
{

}

//[ScriptedImporter(1, ".lua")]
//public class LuaImporter : ScriptedImporter
//{
//    public override void OnImportAsset(AssetImportContext ctx)
//    {
//        //读取文件内容
//        var luaTxt = File.ReadAllText(ctx.assetPath);
//        //转成TextAsset（Unity可识别类型）
//        var assetText = new TextAsset(luaTxt);
//        //将对象assetText添加到导入操作(AssetImportContext)的结果中。
//        ctx.AddObjectToAsset("main obj", assetText);
//        //将对象assetText作为导入操作的主要对象。
//        ctx.SetMainObject(assetText);
//    }
//}

//[ScriptedImporter(1, ".proto")]
//public class LuaProtoImporter : ScriptedImporter
//{
//    public override void OnImportAsset(AssetImportContext ctx)
//    {
//        //读取文件内容
//        var proto = File.ReadAllText(ctx.assetPath);
//        //转成TextAsset（Unity可识别类型）
//        var assetsText = new TextAsset(proto);
//        //将对象assetText添加到导入操作(AssetImportContext)的结果中。
//        ctx.AddObjectToAsset("main obj", assetsText);
//        //将对象assetText作为导入操作的主要对象。
//        ctx.SetMainObject(assetsText);
//    }
//}


//public class LuaPostprocessor : AssetPostprocessor
//{
//    //资源载入之后的处理
//    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//    {
//        foreach (string str in importedAssets)
//        {
//            if (str.EndsWith(".lua"))
//            {
//                //Debug.Log("LuaPostprocessor:" + str);

//                var lua_obj = AssetDatabase.LoadAssetAtPath<Object>(str);

//                AssetDatabase.SetLabels(lua_obj, new string[] { "lua" });
//            }

//            if (str.EndsWith(".proto"))
//            {
//                //Debug.Log("LuaProtoPostprocessor:" + str);

//                var lua_obj = AssetDatabase.LoadAssetAtPath<Object>(str);

//                AssetDatabase.SetLabels(lua_obj, new string[] { "proto" });
//            }
//        }
//    }
//}
