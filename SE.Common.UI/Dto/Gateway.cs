
using System;

namespace SE.Common.UI.Dto
{
    public class Gateway : Device
    {
        public string IP { get; set; }
        public int Port { get; set; }

        public override String GetDeviceType() => "Gateway";
    }
}
