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
//        //��ȡ�ļ�����
//        var luaTxt = File.ReadAllText(ctx.assetPath);
//        //ת��TextAsset��Unity��ʶ�����ͣ�
//        var assetText = new TextAsset(luaTxt);
//        //������assetText��ӵ��������(AssetImportContext)�Ľ���С�
//        ctx.AddObjectToAsset("main obj", assetText);
//        //������assetText��Ϊ�����������Ҫ����
//        ctx.SetMainObject(assetText);
//    }
//}

//[ScriptedImporter(1, ".proto")]
//public class LuaProtoImporter : ScriptedImporter
//{
//    public override void OnImportAsset(AssetImportContext ctx)
//    {
//        //��ȡ�ļ�����
//        var proto = File.ReadAllText(ctx.assetPath);
//        //ת��TextAsset��Unity��ʶ�����ͣ�
//        var assetsText = new TextAsset(proto);
//        //������assetText��ӵ��������(AssetImportContext)�Ľ���С�
//        ctx.AddObjectToAsset("main obj", assetsText);
//        //������assetText��Ϊ�����������Ҫ����
//        ctx.SetMainObject(assetsText);
//    }
//}


//public class LuaPostprocessor : AssetPostprocessor
//{
//    //��Դ����֮��Ĵ���
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
