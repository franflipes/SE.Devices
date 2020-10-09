using SE.Common.UI.Dto;
using System.Collections.Generic;

namespace SE.UI.WebApp.ViewModels
{
    //ViewModel composed by 2 other viewModels
    public class DeviceListViewModel
    {
        public IEnumerable<Gateway> Gateways { get; set; }
        public IEnumerable<Counter> Counters { get; set; }
    }
}
