using NServiceBus;
using System.Threading.Tasks;
using System.Windows;

namespace SE.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IEndpointInstance EndpointInstance { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AsyncStart().GetAwaiter().GetResult();


        }

        async Task AsyncStart()
        {
            var endpointConfiguration = new EndpointConfiguration("SE.UI.WPF");

            endpointConfiguration.UseTransport<LearningTransport>();

            EndpointInstance = await Endpoint.Start(endpointConfiguration)
                            .ConfigureAwait(false);
        }
    }

}
