using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Device
{
    public interface IDevice
    {
        string Name { get; set; }
        string ConnectionParam { get; set; }

        void DeviceOpen();
        void DeviceClose();
        void SetConnectionParam(string param);
    }
}
