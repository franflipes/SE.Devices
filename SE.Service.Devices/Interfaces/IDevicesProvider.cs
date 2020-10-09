using SE.Service.Devices.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SE.Service.Devices.Interfaces
{
    public interface IDevicesProvider
    {
        Task<(bool IsSuccess, IEnumerable<IDevice> Devices, String ErrorMessage)> GetDevicesAsync();
        Task<(int Id, Counter Device, string ErrorMessage)> InsertCounterAsync(Counter counter);
        Task<(int Id, Gateway Device, string ErrorMessage)> InsertGatewayAsync(Gateway counter);
        IEnumerable<String> GetBrands();
        IEnumerable<String> GetModels();
    }
}
