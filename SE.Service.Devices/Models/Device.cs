using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SE.Service.Devices.Enums;
using System;

namespace SE.Service.Devices.Models
{
    public class Device
    {

        public String SerialNumber { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BrandType Brand { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ModelType Model { get; set; }
    }
}
