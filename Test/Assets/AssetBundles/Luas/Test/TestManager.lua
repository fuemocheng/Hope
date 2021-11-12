--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

print ("TestManager")
--require "Test/TestSort"
--require("Test/TestCSFunction")

function OnClickTest()
    LogError("OnClickTest")
    local launcher = CS.UnityEngine.GameObject.Find("Launcher")
    LogError(launcher.name)
end

function TestLogin()
    NetManager.Instance.listener:Add(
        ProtoEnum.cmd_proto.Cmd.LOGIN,
        "key2",
        function(this, msg)
            if msg.errorCode == ProtoEnum.cmd_proto.ErrorCode.SUCCESS then
                LogError("Send Success 2")
            else
                LogError("Send Failed 2")
            end
        end
    )

    NetManager.Instance:SendAndReceive(
        ProtoEnum.cmd_proto.Cmd.LOGIN,
        {token = "", relogin = false, game_id = 1001,},
        "key",
        function (sender, msg)
            if msg.errorCode == ProtoEnum.cmd_proto.ErrorCode.SUCCESS then
                LogError("Send Success")
            else
                LogError("Send Failed")
            end
        end
    )
end