using SE.UI.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SE.UI.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para NewDevice.xaml
    /// </summary>
    public partial class NewDevice : Window
    {
        NewDeviceViewModel _vm;

        public NewDevice()
        {
            InitializeComponent();
            _vm = new NewDeviceViewModel();
            DataContext = _vm;
        }

        private void comboRegistrationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            ComboBox cmb = (ComboBox)sender;
            ComboBoxItem selectedType = (ComboBoxItem)cmb.SelectedItem;
            if (!(selectedType == null))
            {
                if (selectedType.Content.ToString() == "Counter")
                {
                    CounterPanel.Visibility = Visibility.Visible;
                    GatewayPanel.Visibility = Visibility.Hidden;
                }
                else
                {
                    CounterPanel.Visibility = Visibility.Hidden;
                    GatewayPanel.Visibility = Visibility.Visible;
                }
            }

        }

        private void SendCommandDevice_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedType = (ComboBoxItem)comboRegistrationType.SelectedItem;
            if (selectedType.Content.ToString() == "Counter")
            {
                _vm.RegisterCounter().Wait();
            }
            else
            {
                _vm.RegisterGateway().Wait();
            }

        }


    }
}
