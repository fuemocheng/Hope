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
            byteArray.Write(packet.data);
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
                byte[] data;
                byteArray.Read(out data, byteArray.Length - byteArray.Position);
                netPacket.cmd = cmd;
                netPacket.msgid = msgid;
                netPacket.data = data;
            }
            byteArray.Close();
            return netPacket;
        }
    }
}
