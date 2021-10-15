--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

LuaManager = class("LuaManager")
local M = LuaManager

local lastGCTime = 0
local gcTime = 60

function M:ctor()
    -- body
end

function M:Init()
    -- body
    Log("LuaManager->Init!")

end

function M:Update()
    -- print("LuaManager-Update")
    self:CheckGC()
end

function M:Clear()
    -- body
end

function M:CheckGC()
    -- 每分钟强制触发luaGC,防止大量对象GC导致的严重卡顿
    local now = os.time()
    if now - lastGCTime > gcTime then
        collectgarbage("collect")
        lastGCTime = now
    end
end

LuaManager.Instance = M.new()