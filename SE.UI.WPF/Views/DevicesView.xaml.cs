using SE.UI.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SE.UI.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para DevicesView.xaml
    /// </summary>
    public partial class DevicesView : UserControl
    {
        DevicesViewModel _vm;
        public DevicesView()
        {

            InitializeComponent();
            //Create ViewModel and Bind it to view
            _vm = new DevicesViewModel();
            DataContext = _vm;
        }

        private void NewDevice_Click(object sender, RoutedEventArgs e)
        {
            NewDevice w = new NewDevice();
            w.Show();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {

            _vm.RefreshData();
        }
    }
}
