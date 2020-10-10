using SE.Service.Devices.Enums;
using SE.Service.Devices.Interfaces;
using System;

namespace SE.Service.Devices.Models
{
    public class Gateway : Device, IDevice
    {
        public string IP { get; set; }
        public int Port { get; set; }

        public DeviceType GetDeviceType() => DeviceType.Gateway;

        public bool IsValidDevice()
        {
            return !(String.IsNullOrEmpty(SerialNumber) || String.IsNullOrEmpty(IP));
        }
    }
}
