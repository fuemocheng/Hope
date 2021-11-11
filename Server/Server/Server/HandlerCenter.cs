using System;
using NetFrame;
using Server.Logic;
using Server.Logic.Login;
using CmdProto;
using GameProto;
using Google.Protobuf;

namespace Server
{
    public class HandlerCenter : AbsHandlerCenter
    {

        HandlerInterface login;

        public HandlerCenter()
        {
            login = new LoginHandler();
        }

        public override void ClientClose(AsyncUserToken token, string error)
        {
            Console.WriteLine($"[ {token.UserSocket.ToString()} ] 断开连接，{error}");
        }

        public override void ClientConnect(AsyncUserToken token)
        {
            Console.WriteLine($"[ {token.UserSocket.RemoteEndPoint.ToString()} ] 连接");
        }

        public override void MessageReceive(AsyncUserToken token, int cmd, IMessage message)
        {
            Cmd command = (Cmd)cmd;
            switch(command)
            {
                case Cmd.GmCommand:
                    break;
                case Cmd.Login:
                    login.MessageReceive(token, cmd, message);
                    break;
                case Cmd.CreateRole:
                    break;
                case Cmd.SetRolename:
                    break;
                case Cmd.SceneLoad:
                    break;
                case Cmd.SceneRole:
                    break;
                case Cmd.MailOpen:
                    break;
                case Cmd.MailAtch:
                    break;
                case Cmd.MailDel:
                    break;
                default:
                    break;
            }
        }
    }
}
