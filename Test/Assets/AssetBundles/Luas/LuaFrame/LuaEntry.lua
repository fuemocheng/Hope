--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

require "LuaFrame/luaunit"

require "LuaFrame/Class"
require "LuaFrame/BaseDefines"
require "LuaFrame/Utility"
require "LuaFrame/LuaManager"

require "Test/TestManager"


CSStart = function()
    print("CSStart")
end

CSUpdate = function()
    LuaManager.Instance:Update()
end
