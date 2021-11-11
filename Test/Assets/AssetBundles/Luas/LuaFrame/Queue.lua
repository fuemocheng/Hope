---@class Queue
Queue = class("Queue")
local M = Queue
function M:ctor()
    self.first = 0
    self.last = -1
    self.count = 0
end

local function calculateCount(queue)
    queue.count = queue.last - queue.first + 1
end

function M:PushFront(value)
    if value == nil then
        return
    end
    local first = self.first - 1
    self.first = first
    self[first] = value
    calculateCount(self)
end

function M:PushBack(value)
    if value == nil then
        return
    end
    local last = self.last + 1
    self.last = last
    self[last] = value
    calculateCount(self)
end

function M:PopFront()
    local first = self.first
    if (first > self.last) then
        return nil
    end
    local value = self[first]
    self[first] = nil
    self.first = first + 1
    calculateCount(self)
    return value
end

function M:PopBack()
    local last = self.last
    if (self.first > last) then
        return nil
    end
    local value = self[last]
    self[last] = nil
    self.last = last - 1
    calculateCount(self)
    return value
end

function M:Empty()
    if (self.first > self.last) then
        return true
    end
    return false
end

function M:Clear()
    if self:Empty() then
        return
    end
    for i = self.first, self.last, 1 do
        self[i] = nil
    end
    self.first = 0
    self.last = -1
    self.count = 0
end

return M.new()
