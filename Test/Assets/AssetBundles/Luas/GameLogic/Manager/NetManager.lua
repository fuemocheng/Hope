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
    self.sendingCmd = nil
    self.isSending = false

    -- CMD to req
    self.CMDToReq = {}
    -- CMD to ack
    self.CMDToAck = {}
end

---Path : $"Assets/AssetBundles/{path}";
---@param protoName any
function M:LoadProto(protoName)
    return CS.ResourceManager.Instance:Load(protoName, typeof(UnityEngine.TextAsset), ".proto")
end

function M:Init()
    local p = protoc.new()
    p.include_imports = true
    local file
    file = self:LoadProto("Luas/Proto/game")
    if file then
        assert(p:load(file.text, "game.proto"))
    end
    file = self:LoadProto("Luas/Proto/cmd")
    if file then
        assert(p:load(file.text, "cmd.proto"))
    end

    self:ParseCMDProto(p)
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

---尝试从队列里取协议发送,一次只发送一个协议
function M:StartCheckSend()
    if self.isSending then
        return
    end
    if self.sendQueue.count > 0 then
        local data = self.sendQueue:PopFront()

        --构建发送数据
        local msg = {cmd = data.cmd}
        local fieldName = self.CMDToReq[data.cmd]
        if not fieldName then
            LogError("没有找到正确的协议--cmd:" .. data.cmd)
            return
        end
        msg[fieldName] = data.data
        local bytes = pb.encode(".cmd_proto.CommonMessage", msg)

        --C#端Send
        CSNetManager:Send(data.cmd, bytes)

        --数据记录
        self.isSending = true
        self.sendingCmd = data.cmd
        self.sendTime = os.time()

        --注册回调
        if data.callback and data.target then
            self.sendAndReceivelistener:Add(data.cmd, data.target, data.callback)
        end
    end
end

---Send
---@param cmd number int
---@param data IMessage IMessage
function M:Send(cmd, data)
    self:SendAndReceive(cmd, data, nil, nil)
end

---SendAndReceive
---@param cmd number int
---@param data IMessage msg
---@param target Object Object
---@param callback function function
function M:SendAndReceive(cmd, data, target, callback)
    local msg = {
        cmd = cmd,
        data = data,
        target = target,
        callback = callback
    }

    self.sendQueue:PushBack(msg)
    self:StartCheckSend()
end

---comment
---@param cmd number int
---@param data byte[] byte[]
function M:Receive(cmd, data)
    if data then
        local d = assert(pb.decode(".cmd_proto.CommonMessage", data))
        if d and d.cmd and d.errorCode then
            d.cmd = pb.enum(".cmd_proto.Cmd", d.cmd)
            d.errorCode = pb.enum(".cmd_proto.ErrorCode", d.errorCode)

            --发送时注册,返回即删
            self:DispatchEventToReceive(d)
            --注册监听的，不删
            self.listener:Dispatch(d.cmd, d)

            --收到协议 发送下一个
            if d.cmd == self.sendingCmd then
                self.isSending = false
                self:StartCheckSend()
            end
        else
            LogError("收到了未知的服务器消息")
        end
    end
end

function M:DispatchEventToReceive(msg)
    self.sendAndReceivelistener:Dispatch(msg.cmd, msg)
    self.sendAndReceivelistener:Remove(msg.cmd)
end

function M:Update()
    if self.isSending then
        if os.time() - self.sendTime > 30 then
            self.isSending = false
            local resp = {}
            resp.code = self.sendingCmd
            resp.errorCode = ProtoEnum.cmd_proto.ErrorCode.SERVICE_UNAVAILABLE
            self:DispatchEventToReceive(resp)
            LogError("服务器协议超时(等待时间30s) --> cmd: " .. self.sendingCmd)

            --发送下一个
            self:StartCheckSend()
        end
    end
end

NetManager.Instance = M.new()
