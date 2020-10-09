using Microsoft.AspNetCore.Mvc;
using SE.Service.Devices.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Service.Devices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : Controller
    {
        private readonly IDevicesProvider _devicesProvider;

        public DeviceController(IDevicesProvider devicesProvider)
        {
            _devicesProvider = devicesProvider;
        }

        //   api/device
        [HttpGet]
        public async Task<IActionResult> GetDevicesAsync()
        {
            var result = await _devicesProvider.GetDevicesAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Devices);
            }
            return NotFound();
        }


        //We don´t do as Async or take much care about this Action as it returns an Enum, not likely to fail or throw any exception 
        //   api/device/models
        [HttpGet]
        [Route("Models")]
        public IActionResult GetModels()
        {
            var result = _devicesProvider.GetModels();
            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }

        //We don´t do as Async or take much care about this Action as it returns an Enum, not likely to fail or throw any exception
        //   api/device/brands
        [HttpGet]
        [Route("Brands")]
        public IActionResult GetBrands()
        {
            var result = _devicesProvider.GetBrands();
            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
