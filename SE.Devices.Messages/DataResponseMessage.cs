using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Devices.Messages
{
    //It was meant to be used in Req-res or callbacks but 
    public class DataResponseMessage:IMessage
    {
        public int DataId { get; set; }
        public String Message { get; set; }
    }
}
