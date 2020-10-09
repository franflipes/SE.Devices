using NServiceBus;
using System;

namespace SE.Devices.Messages
{
    //Command to be sent to Devices Service which will create the Gateway device
    public class CreateGateway : ICommand
    {
        public String SerialNumber { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }
    }
}
