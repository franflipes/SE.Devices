using System.Collections.Generic;
using System.Threading.Tasks;

namespace SE.Common.UI.Interfaces
{
    /// <summary> Interface
    /// with method to get devices signature
    public interface IDevicesService
    {
        Task<(bool IsSuccess, IEnumerable<IDevice> Devices, string ErrorMessage)> GetDevicesAsync();
    }
}
