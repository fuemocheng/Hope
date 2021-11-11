using Google.Protobuf;
using NetFrame;

namespace Server.Logic
{
    public interface HandlerInterface
    {
        void ClientClose(AsyncUserToken token, string error);

        void ClientConnect(AsyncUserToken token);

        void MessageReceive(AsyncUserToken token, int cmd, IMessage message);
    }
}
