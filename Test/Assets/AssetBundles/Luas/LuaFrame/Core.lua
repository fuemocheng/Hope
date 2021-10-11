--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu fuemocheng@163.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

-- Implementation of lua class


-- No.1
-- https://www.cocos.com/
-- /cocos2d-x-4.0/cocos/scripting/lua-bindings/script/cocos2d/functions.lua
--[[
function class(classname, ...)
    local cls = {__cname = classname}

    -- 这里可以看出lua的cocos模拟的class方法支持多继承
    local supers = {...}

    for _, super in ipairs(supers) do
        local superType = type(super)
        assert(
            superType == "nil" or superType == "table" or superType == "function",
            string.format('class() - create class "%s" with invalid super class type "%s"', classname, superType)
        )

        -- 这里是class继承的第一种情况，继承一个方法（在子类new之前先执行的内建函数__create）
        if superType == "function" then
            assert(
                cls.__create == nil,
                string.format('class() - create class "%s" with more than one creating function', classname)
            )
            -- if super is function, set it to __create
            -- 如果super是一个方法，把super直接复制给内建函数__create
            cls.__create = super

        -- 下面是class的第二，第三种情况，当传过来的值是table类型的时候
        -- 这里需要明确的一点是，在lua里table其实还分两种，一种是lua的table，另一种是c里转换过来的一种类型叫userTable，
        -- 在用tolua++绑定cocos2dx引擎的时候，tolua++会为userTable类型的类在执行create方法的时候为其加入.isclass属性
        -- （大家可以用执行dump(cc.Layer)观察）
        elseif superType == "table" then
            if super[".isclass"] then
                -- super is native class
                assert(
                    cls.__create == nil,
                    string.format(
                        'class() - create class "%s" with more than one creating function or native class',
                        classname
                    )
                )
                -- 这里可以看出这种继承了c结构的子类其实也只是执行子类之前执行了一个创建父类对象的方法
                cls.__create = function()
                    return super:create()
                end
            else
                -- 后面这个是第三种情况，子类是lua继承的父类也是lua，这里有另一个内建函数__supers，他的作用是存放父类数组（父类可能不止一个）
                -- super is pure lua class
                cls.__supers = cls.__supers or {}
                cls.__supers[#cls.__supers + 1] = super
                if not cls.super then
                    -- set first super pure lua class as class.super
                    cls.super = super
                end
            end
        else
            error(string.format('class() - create class "%s" with invalid super type', classname), 0)
        end
    end

    -- __index方法是一个“操作指南”
    -- 下面的__index对于继承功能的模拟也很关键
    -- 更简单的说就是在我调用方法的时候，class是如何实现继承功能的（当子类搜索自己的函数未果，就会按照__index的指引去搜索父类，去寻找方法）
    cls.__index = cls

    -- 如果子类的父类不存在或者只存在只有一个，那么把索引方法指向唯一的父类
    if not cls.__supers or #cls.__supers == 1 then
        setmetatable(cls, {__index = cls.super})
    else
        -- 如果子类的存在多个父类，那么把索引方法会遍历所有的父类
        setmetatable(
            cls,
            {
                __index = function(_, key)
                    local supers = cls.__supers
                    for i = 1, #supers do
                        local super = supers[i]
                        if super[key] then
                            return super[key]
                        end
                    end
                end
            }
        )
    end

    -- 如果类没有构造函数，则添加一个空构造函数
    if not cls.ctor then
        -- add default constructor
        cls.ctor = function()
        end
    end

    -- 这里的new方法是模拟cocos2dx里面的创建对象，下面的代码一目了然
    cls.new = function(...)
        local instance
        if cls.__create then
            instance = cls.__create(...)
        else
            instance = {}
        end
        setmetatableindex(instance, cls)
        instance.class = cls
        instance:ctor(...)
        return instance
    end

    -- 这里只是为了模拟cocos2dx的编程习惯
    cls.create = function(_, ...)
        return cls.new(...)
    end

    -- 最后返回这个类
    return cls
end
--]]


-- No.2
-- https://blog.codingnow.com/cloud/LuaOO
--[[
local _class = {}
function class(super)
    local class_type = {}
    class_type.ctor = false
    class_type.super = super
    class_type.new = function(...)
        local obj = {}
        do
            -- 这里定义了一个递归函数
            local create
            create = function(c, ...)
                if c.super then
                    create(c.super, ...)
                end
                if c.ctor then
                    c.ctor(obj, ...)
                end
            end

            create(class_type, ...)
        end
        setmetatable(obj, {__index = _class[class_type]})
        return obj
    end

    --表vtbl（名字的命名来源于c++类对象中的虚表）
    local vtbl = {}
    _class[class_type] = vtbl

    setmetatable(
        class_type,
        {
            __newindex = function(t, k, v)
                vtbl[k] = v
            end
        }
    )

    if super then
        setmetatable(
            vtbl,
            {
                __index = function(t, k)
                    local ret = _class[super][k]
                    vtbl[k] = ret
                    return ret
                end
            }
        )
    end

    return class_type
end
--]]

-- No.3
-- https://blog.csdn.net/mywcyfl/article/details/37706085
-- https://blog.csdn.net/mywcyfl/article/details/37706247

-- Internal register
local _class = {}

function class(classname, base)
    local class_type = {}

    class_type.__typeName = classname -- 增加typeName
    class_type.__type = "class"
    class_type.ctor = false

    local vtbl = {}
    _class[class_type] = vtbl
    setmetatable(class_type, {__newindex = vtbl, __index = vtbl})

    if base then
        setmetatable(
            vtbl,
            {
                __index = function(t, k)
                    local ret = _class[base][k]
                    vtbl[k] = ret
                    return ret
                end
            }
        )
    end

    class_type.__base = base
    class_type.new = function(...)
        --create a object, dependent on .__createFunc
        local obj = {}
        obj.__base = class_type
        obj.__type = "object"
        do
            local create
            create = function(c, ...)
                if c.__base then
                    create(c.__base, ...)
                end
                if c.ctor then
                    c.ctor(obj, ...)
                end
            end

            create(class_type, ...)
        end

        setmetatable(obj, {__index = _class[class_type]})
        return obj
    end

    class_type.super = function(self, f, ...)
        assert(
            self and self.__type == "object",
            string.format("'self' must be a object when call super(self, '%s', ...)", tostring(f))
        )

        local originBase = self.__base
        --find the first f function that differ from self[f] in the inheritance chain
        local s = originBase
        local base = s.__base
        while base and s[f] == base[f] do
            s = base
            base = base.__base
        end

        assert(
            base and base[f],
            string.format("base class or function cannot be found when call .super(self, '%s', ...)", tostring(f))
        )
        --now base[f] is differ from self[f], but f in base also maybe inherited from base's baseClass
        while base.__base and base[f] == base.__base[f] do
            base = base.__base
        end

        -- If the base also has a baseclass, temporarily set :super to call that baseClass' methods
        -- this is to avoid stack overflow
        if base.__base then
            self.__base = base
        end

        --now, call the super function
        local result = base[f](self, ...)

        --set back
        if base.__base then
            self.__base = originBase
        end

        return result
    end

    return class_type
end
