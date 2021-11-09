local pb = require "pb"
local protoc = require "Luas/LuaFrame/protoc"
local serpent = require "Luas/LuaFrame/serpent"

local netManager = CSNetManagerEx

-------------
-- lua 侧的网络消息收发管理
-- 要注意的是
-- gate_client 中的协议称为网关协议
-- 网关协议通过SendGateway 和 SendGatewayAndReceive 函数发送
-- 网关协议通过gatewayListener接收
--
---@class NetworkManager
NetworkManager = class("NetworkManager")
local M = NetworkManager
-------------

function M:ctor()
    self.sendAndReceivelistener = LuaEventListener.new()
    self.listener = LuaEventListener.new()

    self.gatewaySendAndReceivelistener = LuaEventListener.new()
    self.gatewayListener = LuaEventListener.new()

    self.sendQueue = Queue.new()
    self.sendingCode = nil
    self.isSending = false

    --CMD to req
    self.CMDToReq = {}
    --CMD  to ack
    self.CMDToAck = {}
end

local function LoadProto(textName)
    return CSResourceManager:Load(textName, typeof(UnityEngine.TextAsset), ".proto")
end

function M:Init()
    local p = protoc.new()
    p.include_imports = true
    local file
    file = LoadProto("Luas/Proto/game")
    if file then
        assert(p:load(file.text, "game.proto"))
    end
    file = LoadProto("Luas/Proto/battle")
    if file then
        assert(p:load(file.text, "battle.proto"))
    end
    file = LoadProto("Luas/Proto/cmd")
    if file then
        assert(p:load(file.text, "cmd.proto"))
    end
    file = LoadProto("Luas/Proto/gate_common")
    if file then
        assert(p:load(file.text, "gate_common.proto"))
    end
    file = LoadProto("Luas/Proto/gate_client")
    if file then
        assert(p:load(file.text, "gate_client.proto"))
    end

    self:ParseCMDProto(p)

    -- for name, basename, type in pb.types() do
    --     print(name, basename, type)
    -- end
end

function M:ParseCMDProto(protc)
    -- body
    self.CMDToReq = {}
    self.CMDToAck = {}

    local cmdProto = protc.loaded["cmd.proto"]
    local allCMD = {}
    local cmdFields = {}
    for index, enum in ipairs(cmdProto.enum_type) do
        if enum.name == "Cmd" then
            for index, cmd in ipairs(enum.value) do
                allCMD[#allCMD + 1] = cmd
            end
            break
        end
    end

    for index, msg in ipairs(cmdProto.message_type) do
        if msg.name == "CommonMessage" then
            for index, field in ipairs(msg.field) do
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

---尝试从队列里取协议发送
---一次只发送一个协议
function M:CheckSendMsgToServer()
    if self.isSending then
        return
    end
    if self.sendQueue.count > 0 then
        local data = self.sendQueue:PopFront()
        local msg = {code = data.msgType}
        local fieldName = self.CMDToReq[data.msgType]
        if not fieldName then
            LogError("没有找到正确得协议：cmd:" .. data.msgType)
            return
        end

        msg[fieldName] = data.data
        local bytes = pb.encode(".cmd_proto.CommonMessage", msg)
        --Log("Lua send:"..serpent.block(msg))
        netManager:LuaSend(0, bytes, data.msgType)
        self.sendingCode = data.msgType
        self.isSending = true
        self.sendTime = os.time()
        if data.callback and data.target then
            self.sendAndReceivelistener:Add(data.msgType, data.target, data.callback)
        end
    end
end

function M:Send(msgType, data, maskAllUI)
    local msg = {msgType = msgType, data = data, maskAllUI = maskAllUI}
    self.sendQueue:PushBack(msg)
    self:CheckSendMsgToServer()
end

--
function M:SendAndReceive(msgType, data, target, callback, maskAllUI)
    local msg = {
        msgType = msgType,
        data = data,
        target = target,
        callback = callback,
        maskAllUI = maskAllUI
    }

    self.sendQueue:PushBack(msg)
    self:CheckSendMsgToServer()
end

---comment
---@param msgId number 使用 ProtoEnum.gate_client_proto.ClientMsgType
---@param data table 使用gate_client.proto的结构 请和msgId对应
function M:SendGateway(msgId, data)
    -- body
    local bytes = pb.encode(ProtoGayway.IdToProto[msgId], data)
    netManager:LuaSend(msgId, bytes, 0)
end

---comment
---@param msgId number 使用 ProtoEnum.gate_client_proto.ClientMsgType
---@param data table 使用gate_client.proto的结构 请和msgId对应
---@param target any
---@param callback any
function M:SendGatewayAndReceive(msgId, data, target, callback)
    -- body
    self:SendGateway(msgId, data)
    self.gatewaySendAndReceivelistener:Add(msgId, target, callback)
end

function M:Receive(msgId, data)
    if (msgId == ProtoEnum.gate_client_proto.ClientMsgType.ClientType_GameMsg) then
        -- 因为ping 也走这里，先要处理ping 的问题
        -- CSUIUtility.BlockAllCanvaGroups(false)

        if data then
            local s = os.clock()
            local d = assert(pb.decode(".cmd_proto.CommonMessage", data))
            if d and d.code and d.errorCode then
                d.code = pb.enum(".cmd_proto.Cmd", d.code)
                d.errorCode = pb.enum(".cmd_proto.ErrorCode", d.errorCode)

                self:DispatchEventToReceive(d)
                self.listener:Dispatch(d.code, d)

                --if UNITY_EDITOR then
                --    Log("Lua received time:".. (os.clock() - s) .. serpent.block(d))
                --end

                if d.code == self.sendingCode then
                    --收到协议 发送下一个
                    self.isSending = false
                    self:CheckSendMsgToServer()
                end
            else
                LogError("收到了未知的服务器消息")
            end
        end
    else
        --网关协议
        local msg = assert(pb.decode(ProtoGayway.IdToProto[msgId], data))
        if msg then
            local reqId = ProtoGayway.RespToReq[msgId]
            if reqId then
                self.gatewaySendAndReceivelistener:Dispatch(reqId, msg)
                self.gatewaySendAndReceivelistener:Remove(reqId)
            end
            self.gatewayListener:Dispatch(msgId, msg)
        end
    end
end

function M:DispatchEventToReceive(msg)
    self.sendAndReceivelistener:Dispatch(msg.code, msg)
    self.sendAndReceivelistener:Remove(msg.code)
end

---重连后 清空协议队列，并返回一个错误
function M:ClearSendQueue()
    if self.isSending then
        local resp = {}
        resp.code = self.sendingCode
        resp.errorCode = ProtoEnum.cmd_proto.ErrorCode.SERVICE_UNAVAILABLE
        self:DispatchEventToReceive(resp)
    end

    self.isSending = false
    while self.sendQueue.count > 0 do
        local data = self.sendQueue:PopFront()
        if data.target and data.callback then
            self.sendAndReceivelistener:Add(data.msgType, data.target, data.callback)
            local resp = {}
            resp.code = data.msgType
            resp.errorCode = ProtoEnum.cmd_proto.ErrorCode.SERVICE_UNAVAILABLE
            self:DispatchEventToReceive(resp)
        end
    end
end

--cs调用 连接成功
function M:CSOnConnect()
    self:ClearSendQueue()

    PlayerData.instance:Clear()
    ItemData.instance:Clear()
    MailData.instance:Clear()
end

function M:Update()
    if self.isSending then
        if os.time() - self.sendTime > 30 then
            self.isSending = false
            local resp = {}
            resp.code = self.sendingCode
            resp.errorCode = ProtoEnum.cmd_proto.ErrorCode.SERVICE_UNAVAILABLE
            self:DispatchEventToReceive(resp)
            LogError("服务器协议超时(等待时间30s):cmd：" .. self.sendingCode)
            self:CheckSendMsgToServer()
        end
    end
end

NetworkManager.instance = M.new()
