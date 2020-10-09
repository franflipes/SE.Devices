using NServiceBus;
using System;

namespace SE.Devices.Messages
{
    //Command to be sent to Devices Service which will create the Counter device
    public class CreateCounter : ICommand
    {
        public String SerialNumber { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String Type { get; set; }
    }
}
