--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------


Log = function(...)
    UnityEngine.Debug.Log(...)
end

LogWarning = function(...)
    UnityEngine.Debug.LogWarning(...)
end

LogError = function(...)
    UnityEngine.Debug.LogError(...)
end

-- 字符串分割
-- 摘自 cocos2d-x
-- /cocos2d-x-4.0/cocos/scripting/lua-bindings/script/cocos2d/functions.lua
---@param input string
---@param delimiter string
---@return boolean
function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter=='') then return false end
    local pos,arr = 0, {}
    -- for each divider found
    for st,sp in function() return string.find(input, delimiter, pos, true) end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

-- 字符串分割
-- 摘自 luaunit.lua
---@param delimiter any
---@param text any
---@return any
function strsplit(delimiter, text)
-- Split text into a list consisting of the strings in text, separated
-- by strings matching delimiter (which may _NOT_ be a pattern).
-- Example: strsplit(", ", "Anna, Bob, Charlie, Dolores")
    if delimiter == "" or delimiter == nil then -- this would result in endless loops
        error("delimiter is nil or empty string!")
    end
    if text == nil then
        return nil
    end

    local list, pos, first, last = {}, 1
    while true do
        first, last = text:find(delimiter, pos, true)
        if first then -- found?
            table.insert(list, text:sub(pos, first - 1))
            pos = last + 1
        else
            table.insert(list, text:sub(pos))
            break
        end
    end
    return list
end