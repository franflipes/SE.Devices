using SE.Common.UI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.WebApp.Interfaces
{
    public interface IDevicesService
    {
        Task<(bool IsSuccess, IEnumerable<IDevice> Devices, string ErrorMessage)> GetDevicesAsync();
    }
}
