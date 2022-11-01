﻿using System.Net;
using System.Net.Sockets;

namespace MST.Infra.Utility.Helper;

public static class NetworkHelper
{
    public static string GetAddressIpByDns()
    {
        string? localIP;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint?.Address.ToString();
        }
        return localIP==null?"":localIP;
    }
}
