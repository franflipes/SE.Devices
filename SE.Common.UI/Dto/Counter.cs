
using SE.Common.UI.Interfaces;
using System;

namespace SE.Common.UI.Dto
{
    public class Counter : Device, IDevice
    {

        public String Type { get; set; }


        public override String GetDeviceType() => "Counter";
    }
}
