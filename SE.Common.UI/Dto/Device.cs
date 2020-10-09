
using SE.Common.UI.Interfaces;
using System;


namespace SE.Common.UI.Dto
{
    public class Device : IDevice
    {

        public String SerialNumber { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public virtual string GetDeviceType()
        {
            throw new NotImplementedException();
        }
    }
}
