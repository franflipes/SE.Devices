using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;
using SE.Common.UI.Converter;
using SE.Common.UI.Dto;
using SE.Devices.Messages;

namespace SE.UI.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.Title = "Devices UI Console";

            var endpointConfiguration = new EndpointConfiguration("UI.Console");

            var transport=endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            //string fileName = args[0];
            System.Console.WriteLine("Please, introduce json file path:");
            string fileName = System.Console.ReadLine();
            FileStream fileStream;
            try
            {
                 fileStream= new FileStream(fileName, FileMode.Open);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.ReadLine();
                return;

            }
            finally
            {
                await endpointInstance.Stop();
            }
            //we need to read an existing Json
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string content;
                content = reader.ReadToEnd();

                //we use same deserializer as other UI when GetDevicesAsync because we use also a Json as input file
                var device = JsonConvert.DeserializeObject<Device>(content, new DeviceConverter());

                var options = new SendOptions();
                options.SetDestination("SE.Services.Devices");

                if (device is Counter)
                {
                    CreateCounter comm = new CreateCounter()
                    {
                        SerialNumber = ((Counter)device).SerialNumber,
                        Brand = ((Counter)device).Brand,
                        Model = ((Counter)device).Model,
                        Type = ((Counter)device).Type.ToString()
                    };
                    
                    await endpointInstance.Send(comm,options);
                    
                }
                else 
                {
                    CreateGateway comm = new CreateGateway()
                    {
                        SerialNumber = ((Gateway)device).SerialNumber,
                        Brand = ((Gateway)device).Brand,
                        Model = ((Gateway)device).Model,
                        IP = ((Gateway)device).IP,
                        Port= ((Gateway)device).Port
                    };
                    await endpointInstance.Send(comm,options);
                }
            }
            System.Console.WriteLine("Press Enter to exit.");
            System.Console.ReadLine();

            await endpointInstance.Stop();


        }
    }
}
