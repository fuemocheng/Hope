---@class LuaEventListener
LuaEventListener = class("LuaEventListener")
local M = LuaEventListener

function M:ctor()
    self.listenerTable = {}
end

---添加监听事件 不能重复添加
---@param eventType any eventid
---@param listener any eventOwner
---@param f function callback
function M:Add(eventType, listener, f)
    local t = self.listenerTable[eventType]
    if not t then
        t = {}
        self.listenerTable[eventType] = t
    end
    assert(f, "被监听的方法不能为空")
    t[#t + 1] = {listener = listener, f = f}
end

---同一个eventType，同一个listener，只能有一个回调f
---@param eventType any
---@param listener any
---@param f any
function M:Remove(eventType, listener, f)
    local t = self.listenerTable[eventType]
    if t then
        if listener then
            for i = 1, #t do
                if t[i].listener == listener then
                    table.remove(t, i)
                    break
                end
            end
        elseif f then
            for i = 1, #t do
                if t[i].f == f then
                    table.remove(t, i)
                    break
                end
            end
        else
            self.listenerTable[eventType] = nil
        end
    end
end

---comment 分发消息
---@param eventType any eventid
function M:Dispatch(eventType, arg1, arg2, arg3, arg4, arg5)
    local t = self.listenerTable[eventType]
    if not t then
        return
    end
    for i = 1, #t do
        if t[i] then
            xpcall(
                function()
                    t[i].f(t[i].listener, arg1, arg2, arg3, arg4, arg5)
                end,
                function(msg)
                    LogError(msg)
                end
            )
        end
    end
end

function M:Clear()
    self.listenerTable = {}
end
