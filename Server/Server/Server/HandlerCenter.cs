using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.Coding;
using Server.Logic;
using Server.Logic.Login;
using CmdProto;
using GameProto;

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

        public override void MessageReceive(AsyncUserToken token, NetPacket message)
        {
            Cmd cmd = (Cmd)message.cmd;
            switch(cmd)
            {
                case Cmd.GmCommand:
                    break;
                case Cmd.Login:
                    login.MessageReceive(token, message);
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
