using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;

using SE.Common.UI.Dto;
using SE.Common.UI.Interfaces;
using SE.Common.UI.ViewModels;
using SE.Devices.Messages;
using SE.UI.WebApp.Models;
using SE.UI.WebApp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SE.UI.WebApp.Controllers
{
    /// <summary> Class <c>HomeController</c>
    /// This is the main controller for this simple webApp. We manage most of the exceptions in other layers as business or data
    /// </summary>
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IDevicesService _devicesService;
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageSession _messageSession;


        //Services(deviceService, NServiceBus Session and a logger) Injected
        public HomeController(IDevicesService devicesService, IMessageSession messageSession, ILogger<HomeController> logger)
        {
            _devicesService = devicesService;
            _logger = logger;
            _messageSession = messageSession;
        }

        // Home/index or nothing as index route
        [Route("~/")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [Route("devices")]
        public async Task<IActionResult> Devices()
        {
            var result = await _devicesService.GetDevicesAsync();
            if (result.IsSuccess)
            {
                DeviceListViewModel vm = new DeviceListViewModel()
                {
                    Gateways = result.Devices.OfType<Gateway>(),
                    Counters = result.Devices.OfType<Counter>()
                };
                return View(vm);
            }
            return View("Error", "Device Service currently unavailable");

        }

        [Route("GoRegistration")]
        [HttpPost]
        public IActionResult GoRegistration(RegistrationTypeViewModel vm)
        {
            if (vm.DeviceType == Enums.RegistrationType.Counter)
            {
                
                return RedirectToAction("RegisterCounter");
            }
            else if (vm.DeviceType == Enums.RegistrationType.Gateway)
            {
                
                return RedirectToAction("RegisterGateway");
            }
            return View();
        }

        [Route("RegisterCounter")]
        [HttpGet]
        public async Task<ViewResult> RegisterCounter()
        {
            CounterViewModel vm = new CounterViewModel();
            
            return View(vm);
        }

        [Route("RegisterCounter")]
        [HttpPost]
        public async Task<IActionResult> RegisterCounter(CounterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                CreateCounter comm = new CreateCounter()
                {
                    SerialNumber = vm.SerialNumber,
                    Brand = vm.Brand,
                    Model = vm.Model,
                    Type = vm.CounterType.ToString()
                };
                //first attempt of sending a command didn´t receive anything back as confirmation, so try with Request<tuple>
                //await _messageSession.Send(comm);
                var response = await _messageSession.Request<DataResponseMessage>(comm);
                ViewBag.Message = response.Message;

                //If success, then redirect to devices list and see created one
                if (response.DataId > 0)
                    return RedirectToAction("Devices");
            }
            
            return View();
        }

        [Route("RegisterGateway")]
        [HttpGet]
        public async Task<ViewResult> RegisterGateway()
        {
            GatewayViewModel vm = new GatewayViewModel();
            return View(vm);
        }

        [Route("RegisterGateway")]
        [HttpPost]
        public async Task<IActionResult> RegisterGateway(GatewayViewModel vm)
        {
            if (ModelState.IsValid)
            {
                CreateGateway comm = new CreateGateway()
                {
                    SerialNumber = vm.SerialNumber,
                    Brand = vm.Brand,
                    Model = vm.Model,
                    IP = vm.Ip,
                    Port = vm.Port
                };
                //first attempt of sending a command didn´t receive anything back as confirmation, so try with Request<tuple>
                //await _messageSession.Send(comm);
                var response = await _messageSession.Request<DataResponseMessage>(comm);
                ViewBag.Message = response.Message;

                //If success, then redirect to devices list and see created one
                if (response.DataId > 0)
                    return RedirectToAction("Devices");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
