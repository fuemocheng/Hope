--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------


Log = function(...)
    print(...)
end

LogWarning = function(...)
    print(...)
end

LogError = function(...)
    print(...)
end

-- 字符串分割
-- https://www.cocos.com/
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