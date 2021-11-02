using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public object message { get; set; }

        public NetPacket()
        {

        }

        public T GetMessage<T>()
        {
            return (T)message;
        }
    }
}
