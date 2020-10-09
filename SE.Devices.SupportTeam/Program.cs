using NServiceBus;
using System;
using System.Threading.Tasks;

namespace SE.Devices.SupportTeam
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Devices Support Team";

            var endpointConfiguration = new EndpointConfiguration("SupportTeam");

            endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
