using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SE.Common.UI.Converter;
using SE.Common.UI.Dto;
using SE.Common.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SE.Common.UI.Services
{
    /// <summary> Class <c>DevicesService</c>
    /// This class will help UI to connect to Device Service and make Http calls to make queries 
    /// </summary>

    public class DevicesService : IDevicesService
    {
        private readonly ILogger<DevicesService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary> Ctor 
        /// Initializes a new instance of the <see cref="DevicesService"/> class.
        /// This constructor is intended for .Net core web applications as we inject factory
        /// </summary>
        /// <param name="httpClientFactory">a factory that holds different clients</param>
        /// <param name="logger">will help in logging purposes</param>
        public DevicesService(IHttpClientFactory httpClientFactory, ILogger<DevicesService> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary> Ctor
        /// Initializes a new instance of the <see cref="DevicesService"/> class.
        /// /// This constructor is intended for WPF app as we don´t have DI/// </summary>
        /// <param name="httpClient">an http client</param>
        /// <param name="logger">will help in logging purposes</param>
        public DevicesService(HttpClient httpClient, ILogger<DevicesService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <summary> Method
        /// calls Device Service to request all Devices.
        /// Uses CustomCOnverter as response is heterogeneous
        /// <returns>A boolean if everyting is OK,
        /// <returns>A List of Devices,
        /// <returns>A boolean if everyting is OK,

        public async Task<(bool IsSuccess, IEnumerable<IDevice> Devices, string ErrorMessage)> GetDevicesAsync()
        {
            try
            {
                //private method to select the one is not null, for webApp injected using DI or for WPF injected manually
                var client = GetDevicesServiceClient();

                var response = client.GetAsync("api/Device").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    
                    //Use DeviceConverter to read heterogeneous JSON from Service
                    var devices = JsonConvert.DeserializeObject<IEnumerable<Device>>(content, new DeviceConverter());
                    
                    return (true, (IEnumerable<IDevice>)devices, null);
                }
                return (false, null, response.ReasonPhrase);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        /// <summary> Method
        /// private metthod that decides whether to use Factory or client, but at the end return an http client
        private HttpClient GetDevicesServiceClient()
        {
            if (_httpClientFactory != null)
                return _httpClientFactory.CreateClient("DevicesService");
            return _httpClient;
        }
    }
}
