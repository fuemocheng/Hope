--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

-- luaunit
require "LuaFrame/luaunit"

-- base
require "LuaFrame/Class"
require "LuaFrame/BaseDefines"
require "LuaFrame/luahotupdate"

-- Utility
require "LuaFrame/Utility"
require "LuaFrame/Stack"
require "LuaFrame/Queue"
require "LuaFrame/LuaEventListener"

-- LuaManager
require "LuaFrame/LuaManager"

--GamePlay
require "Test/TestManager"
require "GameLogic/Manager/NetManager"

CSStart = function()
    LuaManager.Instance:Init()
end

CSUpdate = function()
    LuaManager.Instance:Update()
end

CSNetMsgRecv = function(msgId, data)
    NetManager.instance:Receive(msgId, data)
end
