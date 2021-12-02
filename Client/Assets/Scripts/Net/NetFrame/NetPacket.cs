using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using CmdProto;

namespace NetFrame.Coding
{
    public class NetPacket
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public int cmd { get; set; }

        /// <summary>
        /// 留余空间
        /// </summary>
        public int msgid { get; set; }

        /// <summary>
        /// 消息数据 CommonMessage.ToByteArray()
        /// </summary>
        public byte[] data { get; set; }

        public NetPacket() { }

        public NetPacket(int cmd, int msgid, byte[] data)
        {
            this.cmd = cmd;
            this.msgid = msgid;
            this.data = data;
        }
    }
}
