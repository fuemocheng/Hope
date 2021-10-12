--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

LuaManager = class("LuaManager")
local M = LuaManager

function M:ctor()
    -- body
end

function M:Init()
    -- body
end

function M:Update()
    print("LuaManager-Update")
end

function M:Clear()
    -- body
end

function M:CheckGC()
    --body
end

LuaManager.Instance = M.new()