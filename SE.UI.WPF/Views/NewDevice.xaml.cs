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




    }
}
