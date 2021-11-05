using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmdProto;
using Google.Protobuf;

namespace NetFrame.Coding
{
    public class MessageEncoding
    {
        /// <summary>
        /// 消息体序列化
        /// </summary>
        public static byte[] Encode(NetPacket value)
        {
            NetPacket packet = value as NetPacket;
            ByteArray byteArray = new ByteArray();
            //读取数据顺序必须和写入顺序保持一致
            byteArray.Write(packet.cmd);
            byteArray.Write(packet.msgid);
            if (packet.message != null)
            {
                var comMsg = new CommonMessage { Cmd = (Cmd)packet.cmd };
                //回复客户端请求, 或者通知客户端信息
                ProtoUtil.ReqCommonMsg((Cmd)packet.cmd, comMsg, packet.message);
                byteArray.Write(comMsg.ToByteArray());
            }
            byte[] result = byteArray.GetBuffer();
            byteArray.Close();
            return result;
        }

        /// <summary>
        /// 消息体反序列化
        /// </summary>
        public static NetPacket Decode(byte[] value)
        {
            NetPacket netPacket = new NetPacket();
            ByteArray byteArray = new ByteArray(value);

            //从数据中读取MsgId, 读取数据顺序必须和写入顺序保持一致
            int cmd;
            int msgid;
            byteArray.Read(out cmd);
            byteArray.Read(out msgid);

            if (byteArray.Readable)
            {
                byte[] message;
                byteArray.Read(out message, byteArray.Length - byteArray.Position);
                var commonMsg = CommonMessage.Parser.ParseFrom(message);
                //解析客户端的Req
                netPacket.message = ProtoUtil.AckCommonMsg(commonMsg);
            }
            byteArray.Close();
            return netPacket;
        }
    }
}
