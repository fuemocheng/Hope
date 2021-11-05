using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetServerConnection
{
    public string ip;
    public Socket socket;

    protected ByteBuffer sendBuffer = new ByteBuffer(1024);
    protected ByteBuffer receiveBuff = new ByteBuffer(1024);

    public List<NetPacket> receivePacketList = new List<NetPacket>();

    public NetServerConnection()
    {
    }

    public NetServerConnection(Socket _socket)
    {
        socket = _socket;
        socket.BeginReceive(receiveBuff.BBuffer, 0, receiveBuff.Capacity, SocketFlags.None, OnRecive, socket);
    }

    public void Send(NetPacket packet)
    {
        if (!IsSocketValid())
            return;

        sendBuffer.Clear();
        packet.WriteBuffer(0, sendBuffer);
        socket.BeginSend(sendBuffer.BBuffer, 0, sendBuffer.Count, SocketFlags.None, OnSend, socket);
    }

    private void OnSend(IAsyncResult result)
    {
        var len = socket.EndReceive(result);
        sendBuffer.Clear();
    }

    private void OnRecive(IAsyncResult result)
    {
        if (!IsSocketValid())
            return;

        receiveBuff.Count = socket.EndReceive(result);

        var packet = new NetPacket();
        packet.ReadBuffer(receiveBuff);
        receivePacketList.Add(packet);

        receiveBuff.Clear();
        socket.BeginReceive(receiveBuff.BBuffer, 0, receiveBuff.Capacity, SocketFlags.None, OnRecive, socket);
    }

    public bool IsSocketValid()
    {
        if (socket == null || !socket.Connected)
        {
            Close();
            return false;
        }
        return true;
    }

    public void Close()
    {
        if (socket != null)
            socket.Close();

        socket = null;
    }
}