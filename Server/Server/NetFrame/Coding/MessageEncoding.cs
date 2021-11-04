using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace NetFrame.Coding
{
    public class MessageEncoding
    {
        /// <summary>
        /// 
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
                byteArray.Write(packet.message.ToByteArray());
            }
            byte[] result = byteArray.GetBuffer();
            byteArray.Close();
            return result;
        }

        /// <summary>
        /// 
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

            if(byteArray.Readable)
            {
                byte[] message;
                byteArray.Read(out message, byteArray.Length - byteArray.Position);
                netPacket.message = Parser.Parse(cmd, message);
            }
            byteArray.Close();
            return netPacket;
        }
    }
}
