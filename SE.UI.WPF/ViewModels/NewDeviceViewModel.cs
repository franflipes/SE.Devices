using NServiceBus;
using SE.Common.UI.ViewModels;
using SE.Devices.Messages;
using System.Threading.Tasks;

namespace SE.UI.WPF.ViewModels
{
    public class NewDeviceViewModel : ViewModelBase
    {
        public NewDeviceViewModel()
        {
            Counter = new CounterViewModel();
            Gateway = new GatewayViewModel();
        }

        public CounterViewModel Counter { get; set; }
        public GatewayViewModel Gateway { get; set; }

        public async Task RegisterCounter()
        {


            CreateCounter comm = new CreateCounter()
            {
                SerialNumber = Counter.SerialNumber,
                Brand = Counter.Brand,
                Model = Counter.Model,
                Type = Counter.CounterType.ToString()
            };
            var options = new SendOptions();
            options.SetDestination("SE.Services.Devices");
            await App.EndpointInstance.Send(comm, options);

        }

        public async Task RegisterGateway()
        {

            CreateGateway comm = new CreateGateway()
            {
                SerialNumber = Gateway.SerialNumber,
                Brand = Gateway.Brand,
                Model = Gateway.Model,
                IP = Gateway.Ip,
                Port = Gateway.Port
            };
            var options = new SendOptions();
            options.SetDestination("SE.Services.Devices");
            await App.EndpointInstance.Send(comm, options);
        }
    }
}
