using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace SE.Service.Devices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //WebHost default builder and also NServiceBUS configuration
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseNServiceBus(context =>
            {
                // "SE.Services.Devices" endpoint valid for commands and target destination.
                var endpointConfiguration = new EndpointConfiguration("SE.Services.Devices");

                //Enable callbacks for responding WebApp
                endpointConfiguration.EnableCallbacks(makesRequests: false);
                endpointConfiguration.UseTransport<LearningTransport>();

                endpointConfiguration.SendFailedMessagesTo("error");
                endpointConfiguration.AuditProcessedMessagesTo("audit");

                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
