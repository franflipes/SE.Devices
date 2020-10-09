using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SE.Service.Devices.Enums;
using SE.Service.Devices.Interfaces;

namespace SE.Service.Devices.Models
{
    public class Counter : Device, IDevice
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CounterType Type { get; set; }


        public DeviceType GetDeviceType() => DeviceType.Counter;
    }
}
