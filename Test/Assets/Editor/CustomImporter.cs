using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;


//[ScriptedImporter(1, ".md")]
//public class MarkDownImporter : ScriptedImporter
//{
//    public override void OnImportAsset(AssetImportContext ctx)
//    {
//        //��ȡ�ļ�����
//        var md = File.ReadAllText(ctx.assetPath);
//        //ת��TextAsset��Unity��ʶ�����ͣ�
//        var assetsText = new TextAsset(md);
//        //������assetText��ӵ��������(AssetImportContext)�Ľ���С�
//        ctx.AddObjectToAsset("main obj", assetsText);
//        //������assetText��Ϊ�����������Ҫ����
//        ctx.SetMainObject(assetsText);
//    }
//}