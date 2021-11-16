
/*
 *  此文件自动创建，请勿修改
 */
using Google.Protobuf;
using GameProto;
using CmdProto;

namespace NetFrame.Coding
{
    public class ProtoUtil
    {
        public static IMessage ReqCommonMsg(CommonMessage comMsg)
        {
            IMessage retMessage = null;
            var packetCmd = comMsg.Cmd;
            switch (packetCmd)
            {
                
                case Cmd.GmCommand:
                    retMessage = comMsg.GmCommandReq;
                    break;
                
                case Cmd.Login:
                    retMessage = comMsg.LoginReq;
                    break;
                
                case Cmd.CreateRole:
                    retMessage = comMsg.CreateRoleReq;
                    break;
                
                case Cmd.SetRolename:
                    retMessage = comMsg.SetRoleNameReq;
                    break;
                
                case Cmd.SceneLoad:
                    retMessage = comMsg.SceneLoadReq;
                    break;
                
                case Cmd.SceneRole:
                    retMessage = comMsg.SceneRoleReq;
                    break;
                
                case Cmd.MailOpen:
                    retMessage = comMsg.MailOpenReq;
                    break;
                
                case Cmd.MailAtch:
                    retMessage = comMsg.MailAtchReq;
                    break;
                
                case Cmd.MailDel:
                    retMessage = comMsg.MailDelReq;
                    break;
                
                default:
                    break;
            }
            return retMessage;
        }

        public static void AckCommonMsg(Cmd cmd, CommonMessage comMsg, IMessage data = null)
        {
            switch (cmd)
            {
                
                case Cmd.ClientverNtf:
                    comMsg.ClientVerNtf = data as ClientVerNtf ?? new ClientVerNtf();
                    break;
                
                case Cmd.GmCommand:
                    comMsg.GmCommandAck = data as GMCommandAck ?? new GMCommandAck();
                    break;
                
                case Cmd.Login:
                    comMsg.LoginAck = data as LoginAck ?? new LoginAck();
                    break;
                
                case Cmd.CreateRole:
                    comMsg.CreateRoleAck = data as CreateRoleAck ?? new CreateRoleAck();
                    break;
                
                case Cmd.SetRolename:
                    comMsg.SetRoleNameAck = data as SetRoleNameAck ?? new SetRoleNameAck();
                    break;
                
                case Cmd.RoleinfoNtf:
                    comMsg.RoleInfoNtf = data as RoleInfoNtf ?? new RoleInfoNtf();
                    break;
                
                case Cmd.LoginEndNtf:
                    comMsg.LoginEndNtf = data as LoginEndNtf ?? new LoginEndNtf();
                    break;
                
                case Cmd.SceneLoad:
                    comMsg.SceneLoadAck = data as SceneLoadAck ?? new SceneLoadAck();
                    break;
                
                case Cmd.SceneRole:
                    comMsg.SceneRoleAck = data as SceneRoleAck ?? new SceneRoleAck();
                    break;
                
                case Cmd.SceneRoleNtf:
                    comMsg.SceneRoleNtf = data as SceneRoleNtf ?? new SceneRoleNtf();
                    break;
                
                case Cmd.SceneNpcNtf:
                    comMsg.SceneNpcNtf = data as SceneNpcNtf ?? new SceneNpcNtf();
                    break;
                
                case Cmd.MailListNtf:
                    comMsg.MailListNtf = data as MailListNtf ?? new MailListNtf();
                    break;
                
                case Cmd.MailOpen:
                    comMsg.MailOpenAck = data as MailOpenAck ?? new MailOpenAck();
                    break;
                
                case Cmd.MailAtch:
                    comMsg.MailAtchAck = data as MailAtchAck ?? new MailAtchAck();
                    break;
                
                case Cmd.MailDel:
                    comMsg.MailDelAck = data as MailDelAck ?? new MailDelAck();
                    break;
                
                default:
                    break;
            }
        }
    }
}
