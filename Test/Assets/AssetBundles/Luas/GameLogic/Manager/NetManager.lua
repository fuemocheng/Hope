--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

-- 加载pb文件
local pb = require "pb"
--protoc在lua-protobuf的目录里
local protoc = require "LuaFrame/protoc"
local serpent = require "LuaFrame/serpent"

local netManager = CSNetManager

---------------------------
-- lua 侧的网络消息收发管理
---------------------------
---@class NetManager
NetManager = class("NetManager")
local M = NetManager

function M:ctor()
    self.sendAndReceivelistener = LuaEventListener.new()
    self.listener = LuaEventListener.new()

    self.sendQueue = Queue.new()
    self.sendingCode = nil
    self.isSending = false

    -- CMD to req
    self.CMDToReq = {}
    -- CMD to ack
    self.CMDToAck = {}
end

local function LoadProto(protoname)
end

function Init()
end

function M:ParseCMDProto(protc)
    self.CMDToReq = {}
    self.CMDToAck = {}

    local cmdProto = protc.loaded["cmd.proto"]
    local allCMD = {}
    local cmdFields = {}
    for index, enum in ipairs(cmdProto.enum_type) do
        if enum.name == "Cmd" then
            for index2, cmd in ipairs(enum.value) do
                allCMD[#allCMD + 1] = cmd
            end
            break
        end
    end

    for index, msg in ipairs(cmdProto.message_type) do
        if msg.name == "CommonMessage" then
            for index2, field in ipairs(msg.field) do
                cmdFields[field.number] = field.name
            end
            break
        end
    end
    for index, value in ipairs(allCMD) do
        local filedIndex = value.number * 10
        if cmdFields[filedIndex] ~= nil then
            self.CMDToReq[value.number] = cmdFields[filedIndex]
        end
        filedIndex = filedIndex + 1
        if cmdFields[filedIndex] ~= nil then
            self.CMDToAck[value.number] = cmdFields[filedIndex]
        end
    end
end

NetManager.Instance = M.new()
