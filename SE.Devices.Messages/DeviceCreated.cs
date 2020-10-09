using NServiceBus;
using System;

namespace SE.Devices.Messages
{
    //Event to be published once the device is created
    public class DeviceCreated : IEvent
    {
        public int ID { get; set; }
        public String SerialNumber { get; set; }
        public String DeviceType { get; set; }
    }
}
