using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SE.Common.UI.Converter;
using SE.Common.UI.Dto;
using SE.WebApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SE.WebApp
{
    public class DevicesService:IDevicesService
    {
        private readonly ILogger<DevicesService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public DevicesService(IHttpClientFactory httpClientFactory, ILogger<DevicesService> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public DevicesService(HttpClient httpClient, ILogger<DevicesService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<(bool IsSuccess, IEnumerable<IDevice> Devices, string ErrorMessage)> GetDevicesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("DevicesService");
                var response = await client.GetAsync($"api/Device");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    //var result = JsonSerializer.Deserialize<IEnumerable<Dictionary<String,Object>>>(content, options);
                    var devices =  JsonConvert.DeserializeObject<IEnumerable<Device>>(content, new DeviceConverter());
                    //foreach (var r in result) 
                    //{
                    //    object ip;
                    //    var a = r.TryGetValue("ip", out ip);//.GetValueOrDefault("ip");
                    //}
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
    }
}
