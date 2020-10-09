using NServiceBus;
using NServiceBus.Logging;
using SE.Devices.Messages;
using System;
using System.Threading.Tasks;

namespace SE.Devices.SupportTeam
{
    class DeviceCreatedHandler :
        IHandleMessages<DeviceCreated>
    {
        static ILog log = LogManager.GetLogger<DeviceCreatedHandler>();
        static Random random = new Random();

        public Task Handle(DeviceCreated message, IMessageHandlerContext context)
        {
            log.Info($"Support Team has received the device creation of Type {message.DeviceType}. Serial Number = {message.SerialNumber}");
            return Task.CompletedTask;
        }
    }
}
