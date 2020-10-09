
using System;

namespace SE.Common.UI.Interfaces
{
    //This interface is intended to unify Gateways and Counters (deviceTypes)
    public interface IDevice
    {
        String GetDeviceType();
    }
}
