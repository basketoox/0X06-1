using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Motion.Interfaces.Base
{
    public interface INetWork
    {
        AddressFamily AddressFamily { get; }
        bool Connected { get; }
        IPAddress IPAddr { get; set; }
        bool IsBound { get; }
        EndPoint LocalEndPoint { get; }
        int Port { get; set; }
        ProtocolType ProtocolType { get; }
        int ReceiveTimeout { get; set; }
        EndPoint RemoteEndPoint { get; }
        int SendTimeout { get; set; }
        SocketType SocketType { get; }
    }
}
