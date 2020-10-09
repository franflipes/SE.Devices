using SE.UI.WPF.Views;
using System.Windows;

namespace SE.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnDevices_Click(object sender, RoutedEventArgs e)
        {
            ContentControlMainView.Content = new DevicesView();
        }

        private void BtnBrands_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnModels_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
