using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetFrame.Coding;
using Google.Protobuf;

public class NetPool
{
    Queue<NetPacket> recvPacketPool = new Queue<NetPacket>();
    Queue<NetPacket> sendPacketPool = new Queue<NetPacket>();

    public void AddRecvPacket(NetPacket packet)
    {
        lock (recvPacketPool)
        {
            recvPacketPool.Enqueue(packet);
        }
    }

    public NetPacket GetRecvPacket()
    {
        NetPacket packet = null;
        lock (recvPacketPool)
        {
            if (recvPacketPool.Count > 0)
            {
                packet = recvPacketPool.Dequeue();
            }
        }
        return packet;
    }

    public bool HasPacketToSend()
    {
        lock (sendPacketPool)
        {
            return sendPacketPool.Count > 0;
        }
    }

    public NetPacket AddSendPacket(int cmd, int msgId, IMessage message)
    {
        NetPacket packet = null;
        lock (sendPacketPool)
        {
            packet = new NetPacket();
            packet.cmd = cmd;
            packet.msgid = msgId;
            packet.message = message;
            sendPacketPool.Enqueue(packet);
        }
        return packet;
    }

    public NetPacket GetSendPacket()
    {
        NetPacket packet = null;
        lock (sendPacketPool)
        {
            if (sendPacketPool.Count > 0)
                packet = sendPacketPool.Dequeue();
        }
        return packet;
    }


    public void CleanAll()
    {
        recvPacketPool.Clear();
        sendPacketPool.Clear();
    }

}
