using System;
using System.ComponentModel.DataAnnotations;

namespace SE.Common.UI.ViewModels
{
    public class GatewayViewModel : DeviceViewModel
    {
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", ErrorMessage = "Invalid IP Format")]
        public String Ip { get; set; }
        public int Port { get; set; }
    }
}
