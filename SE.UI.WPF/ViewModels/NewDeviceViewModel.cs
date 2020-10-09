using NServiceBus;
using SE.Common.UI.ViewModels;
using SE.Devices.Messages;
using SE.UI.WPF.Commands;
using System;
using System.CodeDom;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SE.UI.WPF.ViewModels
{
    public class NewDeviceViewModel : ViewModelBase
    {
        public NewDeviceViewModel()
        {
            Counter = new CounterViewModel();
            Gateway = new GatewayViewModel();

            RegistrationType = RegistrationType.Counter;

            //Initialize Command and attach an executable func that is bindable to View
            SendCommand = new SendCommand();
            SendCommand.ExecuteFunc = SendRegistration;
            
        }
        
        //SendCommand executable func
        private void SendRegistration()
        {
            if (RegistrationType == RegistrationType.Counter)
            {
                RegisterCounter().Wait();
            }
            else
            {
                RegisterGateway().Wait();
            }
        }

        public SendCommand SendCommand
        {
            get;
            set;
        }

        public CounterViewModel Counter { get; set; }
        public GatewayViewModel Gateway { get; set; }

        //Property that controls the reg type combo via binding, in turn this controls the dockpanels visibility 
        private RegistrationType _registrationType;
        public RegistrationType RegistrationType
        {
            get { return _registrationType; }
            set 
            {
                _registrationType = value;
                if (_registrationType == RegistrationType.Counter)
                {
                    StackCounterVisibility = Visibility.Visible;
                    StackGatewayVisibility = Visibility.Hidden;
                }    
                else if (_registrationType == RegistrationType.Gateway)
                {
                    StackCounterVisibility = Visibility.Hidden;
                    StackGatewayVisibility = Visibility.Visible;
                }
            }
        }

        #region Visibility StackPanel properties
        private Visibility _stackCounterVisibility;
        public Visibility StackCounterVisibility
        {
            get
            {
                return _stackCounterVisibility;
            }
            set
            {
                _stackCounterVisibility = value;
                RaisePropertyChanged(()=>StackCounterVisibility);
            }
        }

        private Visibility _stackGatewayVisibility;
        public Visibility StackGatewayVisibility
        {
            get
            {
                return _stackGatewayVisibility;
            }
            set
            {
                _stackGatewayVisibility = value;
                RaisePropertyChanged(() => StackGatewayVisibility);
            }
        }
        #endregion


        #region  NServiceBus send Method
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
        #endregion
    }

    public enum RegistrationType
    {
        Counter,
        Gateway
    }
}
