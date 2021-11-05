using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static class NetExtension
{
    public static string GetAddressAndPort(this Socket socket)
    {
        var ipEndPort = (socket.RemoteEndPoint ?? socket.LocalEndPoint) as IPEndPoint;
        return ipEndPort.Address.ToString() + ":" + ipEndPort.Port;
    }

    public static bool InsIdEqual(this int self, int other)
    {
        if (self < 0)
            return false;
        return self == other;
    }
}