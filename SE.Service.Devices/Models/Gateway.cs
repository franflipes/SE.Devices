using SE.Service.Devices.Enums;
using SE.Service.Devices.Interfaces;

namespace SE.Service.Devices.Models
{
    public class Gateway : Device, IDevice
    {
        public string IP { get; set; }
        public int Port { get; set; }

        public DeviceType GetDeviceType() => DeviceType.Gateway;
    }
}
