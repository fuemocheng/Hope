--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

LuaManager = class("LuaManager")
local M = LuaManager

local lastGCTime = 0
local gcTime = 5 * 60 --每5分钟强制触发luagc

function M:ctor()
end

function M:Init()
    -- Log("LuaManager->Init")
    self:AddModules()
end

function M:AddModules()
    self.modules = {
        NetManager.Instance,
    }
    for key, module in ipairs(self.modules) do
        if module.Init then
            module:Init()
        end
    end
end

function M:Update()
    -- print("LuaManager-Update")
    self:CheckGC()

    -- Module Update
    for key, module in ipairs(self.modules) do
        if module.Update then
            module:Update()
        end
    end
end

function M:Clear()
    self.modules = {}
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
