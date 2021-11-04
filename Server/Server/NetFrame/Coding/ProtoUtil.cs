using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using GameProto;
using CmdProto;

/// <summary>
/// 消息解析器，手动填写，后期可自动生成
/// </summary>
namespace NetFrame.Coding
{
    public class ProtoUtil
    {
        public static void ReqCommonMsg(Cmd cmd, CommonMessage comMsg, IMessage data = null)
        {
            switch (cmd)
            {
                case Cmd.GmCommand:
                    comMsg.GmCommandReq = data as GMCommandReq ?? new GMCommandReq();
                    break;
                case Cmd.Login:
                    comMsg.LoginReq = data as LoginReq ?? new LoginReq();
                    break;
                case Cmd.CreateRole:
                    comMsg.CreateRoleReq = data as CreateRoleReq ?? new CreateRoleReq();
                    break;
                case Cmd.SetRolename:
                    comMsg.SetRoleNameReq = data as SetRoleNameReq ?? new SetRoleNameReq();
                    break;
                case Cmd.SceneLoad:
                    comMsg.SceneLoadReq = data as SceneLoadReq ?? new SceneLoadReq();
                    break;
                case Cmd.SceneRole:
                    comMsg.SceneRoleReq = data as SceneRoleReq ?? new SceneRoleReq();
                    break;
                case Cmd.MailOpen:
                    comMsg.MailOpenReq = data as MailOpenReq ?? new MailOpenReq();
                    break;
                case Cmd.MailAtch:
                    comMsg.MailAtchReq = data as MailAtchReq ?? new MailAtchReq();
                    break;
                case Cmd.MailDel:
                    comMsg.MailDelReq = data as MailDelReq ?? new MailDelReq();
                    break;
                default:
                    break;
            }
        }

        public static IMessage AckCommonMsg(CommonMessage commonMsg)
        {
            IMessage retMessage = null;
            var packetCmd = commonMsg.Cmd;

            switch (packetCmd)
            {
                case Cmd.ClientverNtf:
                    retMessage = commonMsg.ClientVerNtf;
                    break;
                case Cmd.GmCommand:
                    retMessage = commonMsg.GmCommandAck;
                    break;
                case Cmd.Login:
                    retMessage = commonMsg.LoginAck;
                    break;
                case Cmd.CreateRole:
                    retMessage = commonMsg.CreateRoleAck;
                    break;
                case Cmd.SetRolename:
                    retMessage = commonMsg.SetRoleNameAck;
                    break;
                case Cmd.RoleinfoNtf:
                    retMessage = commonMsg.RoleInfoNtf;
                    break;
                case Cmd.LoginEndNtf:
                    retMessage = commonMsg.LoginEndNtf;
                    break;
                case Cmd.SceneLoad:
                    retMessage = commonMsg.SceneLoadAck;
                    break;
                case Cmd.SceneRole:
                    retMessage = commonMsg.SceneRoleAck;
                    break;
                case Cmd.SceneRoleNtf:
                    retMessage = commonMsg.SceneRoleNtf;
                    break;
                case Cmd.SceneNpcNtf:
                    retMessage = commonMsg.CreateRoleAck;
                    break;
                case Cmd.MailListNtf:
                    retMessage = commonMsg.MailListNtf;
                    break;
                case Cmd.MailOpen:
                    retMessage = commonMsg.MailOpenAck;
                    break;
                case Cmd.MailAtch:
                    retMessage = commonMsg.MailAtchAck;
                    break;
                case Cmd.MailDel:
                    retMessage = commonMsg.MailDelAck;
                    break;
                default:
                    break;
            }
            return retMessage;
        }

    }
}
