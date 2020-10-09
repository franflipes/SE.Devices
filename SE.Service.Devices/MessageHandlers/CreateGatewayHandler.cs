using NServiceBus;
using NServiceBus.Logging;
using SE.Devices.Messages;
using SE.Service.Devices.Enums;
using SE.Service.Devices.Interfaces;
using SE.Service.Devices.Models;
using System;
using System.Threading.Tasks;

namespace SE.Service.Devices.MessageHandlers
{
    /// <summary> Class <c>CreateGatewayHandler</c>
    /// This class will handle the create gateway command, will reply with bad/right deending on success and also will publish an event if everithing OK
    /// </summary>
    public class CreateGatewayHandler :
        IHandleMessages<CreateGateway>
    {
        static ILog log = LogManager.GetLogger<CreateCounter>();
        static Random random = new Random();
        private readonly IDevicesProvider _devicesProvider;

        public CreateGatewayHandler(IDevicesProvider deviceProvider)
        {
            _devicesProvider = deviceProvider;

        }

        public Task Handle(CreateGateway message, IMessageHandlerContext context)
        {
            log.Info($"Received Message, Gateway with Serial Number = {message.SerialNumber}");

            // This is normally where some business logic would occur

            BrandType brandType = BrandType.UNKNOWN;
            ModelType modelType = ModelType.UNKNOWN;

            //We safely manage Brands and Models coming with different names as good ones. 
            //Only Devices Services knows for sure the names of the Brands and Model
            //Actually Brands and models shouldn´t be in an ENUMS, 
            //there might be somewhere else stored with the possibility of growing as it is DATA of the domain
            //Check webApi as 2 methods ready to send Brands and Models via REST

            if (message.Brand != null)
                brandType = Enum.TryParse<BrandType>(message.Brand.ToUpper(), out brandType) ? brandType : BrandType.UNKNOWN;
            if (message.Model != null)
                modelType = Enum.TryParse<ModelType>(message.Model.ToUpper(), out modelType) ? modelType : ModelType.UNKNOWN;

            //create model from message and transformations done perviously
            var gateway = new Gateway()
            {
                SerialNumber = message.SerialNumber,
                Brand = brandType,
                Model = modelType,
                IP = message.IP,
                Port = message.Port
            };
            //send model to devicesProvider which will manage creation
            var result = _devicesProvider.InsertGatewayAsync(gateway).Result;

            if (result.Id < 0)
            {
                log.Info($"Gateway couldn´t be created. Problem = {result.ErrorMessage}");

                var badResponse = new DataResponseMessage
                {
                    DataId = -1,
                    Message = result.ErrorMessage
                };

                //send callback with -1 as not created and error message
                return context.Reply(badResponse);
                
                //return Task.CompletedTask;
            }      
            
            //send good response to origin with new id in DB
            var goodResponse = new DataResponseMessage
            {
                DataId = result.Id,
                Message = $"Gateway created with ID:{result.Id}"
            };
            context.Reply(goodResponse).Wait();

            //Create an event message with ID,SN and Counter Type(electricity/water)
            DeviceCreated dc = new DeviceCreated()
            {
                ID = result.Id,
                SerialNumber = result.Device.SerialNumber,
                DeviceType = $"Gateway"
            };
            //publish deviceCreated event to notify subscribers 
            return context.Publish(dc);
        }
    }
}
