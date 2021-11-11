using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmdProto;
using GameProto;
using Google.Protobuf;
using NetFrame;
using NetFrame.Coding;

namespace Server.Logic.Login
{
    public class LoginHandler : HandlerInterface
    {
        public void ClientClose(AsyncUserToken token, string error)
        {

        }

        public void ClientConnect(AsyncUserToken token)
        {

        }

        public void MessageReceive(AsyncUserToken token, int cmd, IMessage message)
        {
            Cmd command = (Cmd)cmd;
            switch (command)
            {
                case Cmd.GmCommand:
                    break;
                case Cmd.Login:
                    Login(token, message as LoginReq);
                    break;
                case Cmd.CreateRole:
                    break;
                case Cmd.SetRolename:
                    break;
               
                default:
                    break;
            }
        }

        public void Login(AsyncUserToken token, LoginReq loginReq)
        {
            //TODO:处理登录逻辑
            //检查是否有此账户
            //有：展示角色信息
            //没有：创建新角色
            string clientToken = loginReq.Token;
            bool reLogin = loginReq.Relogin;

            Console.WriteLine("LoginHandler -> LoginAck");

            //反回客户端创建新角色
            LoginAck loginAck = new LoginAck { CreateRole = true };
            token.Send(Cmd.Login, loginAck);
        }
    }
}
