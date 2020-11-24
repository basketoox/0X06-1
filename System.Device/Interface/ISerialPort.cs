using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace System.Device
{
    public interface ISerialPort
    {
        int BaudRate { get; set; }
        int DataBits { get; set; }
        bool DtrEnable { get; set; }
        bool IsOpen { get; }
        Parity Parity { get; set; }
        string PortName { get; set; }
        int ReadTimeout { get; set; }
        bool RtsEnable { get; set; }
        StopBits StopBits { get; set; }
        int WriteTimeout { get; set; }
    }
}
