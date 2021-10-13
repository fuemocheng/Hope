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