-- Stack Table
-- Uses a table as stack, use <table>:push(value) and <table>:pop()
-- Lua 5.1 compatible

-- GLOBAL
Stack = {}

-- Create a Table with stack functions
function Stack:Create()
    -- stack table
    local t = {}
    -- entry table
    t._et = {}

    -- push a value on to the stack
    function t:push(value)
        table.insert(self._et, value)
    end

    -- pop a value from the stack
    function t:pop()
        local value = nil
        if #self._et ~= 0 then
            value = self._et[#self._et]
            table.remove(self._et)
        end
        return value
    end

    function t:clear()
        for k, v in ipairs(self._et) do
            v = nil
        end
        self._et = {}
    end

    return t
end
