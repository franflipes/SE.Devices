using SE.Common.UI.Dto;
using SE.Common.UI.Interfaces;
using SE.Common.UI.Services;
using SE.UI.WPF.Properties;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace SE.UI.WPF.ViewModels
{
    public class DevicesViewModel : ViewModelBase
    {
        private readonly IDevicesService _deviceService;
        public DevicesViewModel()
        {
            HttpClient client = GetHttpClient();
            //A lot easier with .net COre DI, in WPF we do it manually the injection and creation of objects
            _deviceService = new DevicesService(client, null);

            //call the device service to get Devices
            RefreshData();

        }

        #region bindings

        public ObservableCollection<Gateway> Gateways { get; set; }
        public ObservableCollection<Counter> Counters { get; set; }
        #endregion

        //call the device service to get Devices
        //if result success then populate ObservableCollection to autmatically update UI via Bindings
        public void RefreshData()
        {
            var resultOfDevices = _deviceService.GetDevicesAsync().Result;

            if (resultOfDevices.IsSuccess)
            {
                Gateways = new ObservableCollection<Gateway>(resultOfDevices.Devices.OfType<Gateway>());
                Counters = new ObservableCollection<Counter>(resultOfDevices.Devices.OfType<Counter>());
            }
            else
            {
                MessageBox.Show(!String.IsNullOrEmpty(resultOfDevices.ErrorMessage) ? resultOfDevices.ErrorMessage : "Device Service Unavailable");
            }
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();

            string webApiUri = Settings.Default.DeviceServices;
            //ConfigurationManager.AppSettings.Get("WebApiAddreess");
            client.BaseAddress = new Uri(webApiUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }


    }
}
