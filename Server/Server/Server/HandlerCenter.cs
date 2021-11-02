using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;

namespace Server
{
    public class HandlerCenter : AbsHandlerCenter
    {
        public override void ClientClose(AsyncUserToken token, string error)
        {
            Console.WriteLine($"[ {token.UserSocket.ToString()} ] 断开连接，{error}");
        }

        public override void ClientConnect(AsyncUserToken token)
        {
            Console.WriteLine($"[ {token.UserSocket.RemoteEndPoint.ToString()} ] 连接");
        }

        public override void MessageReceive(AsyncUserToken token, object message)
        {

        }
    }
}
