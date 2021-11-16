using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using CmdProto;
using GameProto;
using System.Text.RegularExpressions;

public class TopMenu
{

    #region Client ProtoUtil Source String

    static string clientProtoUtilSrcStr = @"
/*
 *  此文件自动创建，请勿修改
 */

using Google.Protobuf;
using GameProto;
using CmdProto;

namespace NetFrame.Coding
{{
    public class ProtoUtil
    {{
        public static void ReqCommonMsg(Cmd cmd, CommonMessage comMsg, IMessage data = null)
        {{
            switch (cmd)
            {{
                {0}
                default:
                    break;
            }}
        }}

        public static IMessage AckCommonMsg(CommonMessage comMsg)
        {{
            IMessage retMessage = null;
            var packetCmd = comMsg.Cmd;
            switch (packetCmd)
            {{
                {1}
                default:
                    break;
            }}
            return retMessage;
        }}
    }}
}}";

    static string clientReqStr = @"
                case Cmd.{0}:
                    comMsg.{1} = data as {2} ?? new {2}();
                    break;
                ";

    static string clientAckStr = @"
                case Cmd.{0}:
                    retMessage = comMsg.{1};
                    break;
                ";
    #endregion

    #region Server ProtoUtil Source String
    static string serverProtoUtilSrcStr = @"
/*
 *  此文件自动创建，请勿修改
 */
using Google.Protobuf;
using GameProto;
using CmdProto;

namespace NetFrame.Coding
{{
    public class ProtoUtil
    {{
        public static IMessage ReqCommonMsg(CommonMessage comMsg)
        {{
            IMessage retMessage = null;
            var packetCmd = comMsg.Cmd;
            switch (packetCmd)
            {{
                {0}
                default:
                    break;
            }}
            return retMessage;
        }}

        public static void AckCommonMsg(Cmd cmd, CommonMessage comMsg, IMessage data = null)
        {{
            switch (cmd)
            {{
                {1}
                default:
                    break;
            }}
        }}
    }}
}}
";

    static string serverReqStr = @"
                case Cmd.{0}:
                    retMessage = comMsg.{1};
                    break;
                ";

    static string serverAckStr = @"
                case Cmd.{0}:
                    comMsg.{1} = data as {2} ?? new {2}();
                    break;
                ";

    #endregion


    [MenuItem("Tools/GenProtoUtil")]
    public static void GenProtoUtil()
    {
        GenClientProtoUtil();
        GenServerProtoUtil();
        GenLuaProto();
    }

    static void GenClientProtoUtil()
    {
        using (FileStream fs = new FileStream("Assets/Scripts/Net/NetFrame/ProtoUtil.cs", FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            StringBuilder cmdReqStrBuilder = new StringBuilder();
            StringBuilder cmdAckStrBuilder = new StringBuilder();

            Array cmdEnumArr = System.Enum.GetValues(typeof(Cmd));
            foreach (int cmd in cmdEnumArr)
            {
                int filedNum = cmd * 10;
                string cmdStr = ((Cmd)cmd).ToString();
                var desc = CommonMessage.Descriptor.FindFieldByNumber(filedNum);
                if (desc != null && desc.FieldType == Google.Protobuf.Reflection.FieldType.Message)
                {
                    cmdReqStrBuilder.AppendFormat(clientReqStr, cmdStr, desc.Name.FirstCharToUpper(), desc.MessageType.Name);
                }

                filedNum += 1;
                desc = CommonMessage.Descriptor.FindFieldByNumber(filedNum);
                if (desc != null && desc.FieldType == Google.Protobuf.Reflection.FieldType.Message)
                {
                    cmdAckStrBuilder.AppendFormat(clientAckStr, cmdStr, desc.Name.FirstCharToUpper());
                }
            }
            string result = string.Format(clientProtoUtilSrcStr, cmdReqStrBuilder.ToString(), cmdAckStrBuilder.ToString());

            StreamWriter sw = new StreamWriter(fs);
            sw.Write(result);
            sw.Close();
        }
        LogUtils.LogError("GenClientProtoUtil success!");
    }

    static void GenServerProtoUtil()
    {
        using (FileStream fs = new FileStream("../Server/Server/NetFrame/Coding/ProtoUtil.cs", FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            StringBuilder cmdReqStrBuilder = new StringBuilder();
            StringBuilder cmdAckStrBuilder = new StringBuilder();

            Array cmdEnumArr = System.Enum.GetValues(typeof(Cmd));
            foreach (int cmd in cmdEnumArr)
            {
                int filedNum = cmd * 10;
                string cmdStr = ((Cmd)cmd).ToString();
                var desc = CommonMessage.Descriptor.FindFieldByNumber(filedNum);
                if (desc != null && desc.FieldType == Google.Protobuf.Reflection.FieldType.Message)
                {
                    cmdReqStrBuilder.AppendFormat(serverReqStr, cmdStr, desc.Name.FirstCharToUpper());
                }

                filedNum += 1;
                desc = CommonMessage.Descriptor.FindFieldByNumber(filedNum);
                if (desc != null && desc.FieldType == Google.Protobuf.Reflection.FieldType.Message)
                {
                    cmdAckStrBuilder.AppendFormat(serverAckStr, cmdStr, desc.Name.FirstCharToUpper(), desc.MessageType.Name);
                }
            }
            string result = string.Format(serverProtoUtilSrcStr, cmdReqStrBuilder.ToString(), cmdAckStrBuilder.ToString());

            StreamWriter sw = new StreamWriter(fs);
            sw.Write(result);
            sw.Close();
        }
        LogUtils.LogError("GenServerProtoUtil success!");
    }

    static void GenLuaProto()
    {
        //拷贝Proto
        {
            void CopyTo(string src, string dst)
            {
                FileInfo file = new FileInfo(src);
                if (file.Exists)
                {
                    // true is overwrite 
                    File.SetAttributes(dst, FileAttributes.Normal);
                    file.CopyTo(dst, true);
                }
            }
            string[] protos = new string[] { "cmd", "game" };
            foreach (var proto in protos)
            {
                string sourceFile = $"../Config/Protobuf/proto/{proto}.proto";
                string destinationFile = $"Assets/AssetBundles/Luas/Proto/{proto}.proto";
                CopyTo(sourceFile, destinationFile);
            }
            AssetDatabase.Refresh();
        }

        // 处理解析枚举定义
        // File.OpenWrite替换为File.Open
        // 即FileMode.OpenOrCreate => FileMode.Create
        // 文件存在的时候会整体覆盖
        // 参照:https://docs.microsoft.com/en-us/dotnet/api/system.io.filemode?view=net-5.0
        using (var fs = File.Open("Assets/AssetBundles/Luas/LuaFrame/ProtoEnum.lua", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var sw = new StreamWriter(fs);

            sw.Write(@"
---------------------------
--- 此文件自动创建，请勿修改
---------------------------

ProtoEnum = {
    cmd_proto = { },
    game_proto = { }
}");
            sw.Write("\n\n");

            WriteText("Assets/AssetBundles/Luas/Proto/cmd.proto", "cmd_proto");
            WriteText("Assets/AssetBundles/Luas/Proto/game.proto", "game_proto");

            void WriteText(string path, string head)
            {
                var text = File.ReadAllText(Path.GetFullPath(path));
                var pattern = @"^enum +([\w]+)[\s]*([\w\W]*?}$?)";
                var reg = new Regex(pattern, RegexOptions.Multiline);
                var matches = reg.Matches(text);
                foreach (Match item in matches)
                {
                    var s = item.Groups[2].Value;
                    s = s.Replace("//", "--");
                    sw.Write("ProtoEnum." + head + "." + item.Groups[1].Value + " = " + s + "\n");
                    sw.Write("\n\n");
                }
            }

            sw.Close();
        }

        // 处理message类型 方便查看
        using (var fs = File.Open("Assets/AssetBundles/Luas/LuaFrame/ProtoMessage.lua", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var sw = new StreamWriter(fs);

            sw.Write("--------------------\n");
            sw.Write("-- " + DateTime.Now.ToString() + "\n");
            sw.Write("--------------------\n");

            WriteText("Assets/AssetBundles/Luas/Proto/game.proto", "game_proto");

            void WriteText(string path, string srcProto)
            {
                var text = File.ReadAllText(Path.GetFullPath(path));
                var pattern = @"^message +([\w]+)";
                var reg = new Regex(pattern, RegexOptions.Multiline);
                var matches = reg.Matches(text);
                foreach (Match item in matches)
                {
                    var s = item.Groups[1].Value;
                    sw.Write("---@class " + item.Groups[1].Value + "\n");
                    //Debug.Log(s);
                }
            }
            sw.Close();
        }

        AssetDatabase.Refresh();

        LogUtils.LogError("GenLuaProtoEnum success!");
    }
}
