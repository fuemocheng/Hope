using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using NetFrame;
using NetFrame.Coding;

namespace Server.Logic
{
    public interface HandlerInterface
    {
        void ClientClose(AsyncUserToken token, string error);

        void ClientConnect(AsyncUserToken token);

        void MessageReceive(AsyncUserToken token, NetPacket message);
    }
}
