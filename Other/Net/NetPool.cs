using System;
using System.Collections.Generic;
using System.Threading;

public class NetPool
{
    Queue<NetPacket> recvPacketPool = new Queue<NetPacket>();
    Queue<NetPacket> sendPacketPool = new Queue<NetPacket>();

    public AutoResetEvent sendEvent = new AutoResetEvent(false);

    internal void AddRecvPacket(NetPacket packet)
    {
        lock (recvPacketPool)
        {
            recvPacketPool.Enqueue(packet);
        }
    }

    internal NetPacket GetRecvPacket()
    {
        NetPacket packet = null;
        lock (recvPacketPool)
        {
            if (recvPacketPool.Count > 0)
                packet = recvPacketPool.Dequeue();
        }

        return packet;
    }

    internal void CleanAll()
    {
        LogUtils.Log("CleanAll", sendPacketPool.Count);
        //lock (sendPacketPool)
        {
            sendPacketPool.Clear();
        }
        //lock (recvPacketPool)
        {
            recvPacketPool.Clear();
        }
        lock (this)
        {
            sendEvent.Reset();
        }
    }

    internal NetPacket AddSendPacket(int msgNo, byte[] data, int msgid)
    {
        NetPacket p = null;
        lock (sendPacketPool)
        {
            p = new NetPacket();
            p.cmd = msgNo;
            p.data = data;
            p.msgid = msgid;
            sendPacketPool.Enqueue(p);
        }
        sendEvent.Set();
        return p;
    }

    internal NetPacket GetSendPacket()
    {
        NetPacket packet = null;
        lock (sendPacketPool)
        {
            if (sendPacketPool.Count > 0)
                packet = sendPacketPool.Dequeue();
        }
        return packet;
    }

    internal bool HasPacketToSend()
    {
        lock (sendPacketPool)
        {
            return (sendPacketPool.Count > 0);
        }
    }
}