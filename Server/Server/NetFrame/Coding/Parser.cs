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
    public class Parser
    {
        public static IMessage Parse(int msgID, byte[] data)
        {
            var cmd = (Cmd)msgID;
            IMessage msg = null;
            MessageParser parser = null;
            switch (cmd)
            {
                case CmdProto.Cmd.GameLogin:
                    parser = LoginGameAck.Parser;
                    msg = parser.ParseFrom(data);
                    break;
                case CmdProto.Cmd.GameCreateRole:
                    parser = RoleResume.Parser;
                    msg = parser.ParseFrom(data);
                    break;
                default:
                    break;
            }
            return msg;
        }

    }
}
