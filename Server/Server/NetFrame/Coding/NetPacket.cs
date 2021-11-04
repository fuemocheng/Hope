using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace NetFrame.Coding
{
    public class NetPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public int cmd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int msgid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IMessage message { get; set; }

        public NetPacket() { }

        public NetPacket(int cmd, int msgid, IMessage message)
        {
            this.cmd = cmd;
            this.msgid = msgid;
            this.message = message;
        }

        public T GetMessge<T>() where T : IMessage
        {
            return (T)message;
        }
    }
}
