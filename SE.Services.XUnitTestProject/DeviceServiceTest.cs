using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SE.Service.Devices.Db;
using SE.Service.Devices.Profile;
using SE.Service.Devices.Providers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SE.Service.Devices.XUnitTestProject
{
    public class DeviceServiceTest
    {
        [Fact]
        public async Task GetDevicesReturnAllDevices()
        {
            //use in-memory isolated db different than the one in Device Service
            var options = new DbContextOptionsBuilder<DevicesDbContext>()
                .UseInMemoryDatabase(nameof(GetDevicesReturnAllDevices))
                .Options;
            using (var dbContext = new DevicesDbContext(options))
            {
                //sample data
                CreateDevices(dbContext);

                //We need to add the Profiles we want to use in the mapper; 
                var devicesProfile = new DeviceProfile();
                var configuration = new MapperConfiguration(cfg => cfg.AddProfile(devicesProfile));
                var mapper = new Mapper(configuration);

                var devicesProvider = new DevicesProvider(dbContext, null, mapper);

                var devices = await devicesProvider.GetDevicesAsync();

                //Assert the returned tuple
                Assert.True(devices.IsSuccess);
                Assert.True(devices.Devices.Any());
                Assert.Null(devices.ErrorMessage);
            }
        }

        [Fact]
        public async Task InsertDeviceExistingUniquenessConstraint()
        {
            //use in-memory isolated db different than the one in Device Service
            var options = new DbContextOptionsBuilder<DevicesDbContext>()
                .UseInMemoryDatabase(nameof(GetDevicesReturnAllDevices))
                .Options;
            using (var dbContext = new DevicesDbContext(options))
            {
                //sample data
                CreateDevices(dbContext);

                //We need to add the Profiles we want to use in the mapper; 
                var devicesProfile = new DeviceProfile();
                var configuration = new MapperConfiguration(cfg => cfg.AddProfile(devicesProfile));
                var mapper = new Mapper(configuration);

                var devicesProvider = new DevicesProvider(dbContext, null, mapper);

                //same as SN1 in-memory then check Asserts
                var counter = new Models.Counter()
                {
                    SerialNumber = $"SN1",
                    Type = Enums.CounterType.Water
                };

                var device = await devicesProvider.InsertCounterAsync(counter);

                //Assert the returned tuple
                Assert.True(device.Id == -1);
                Assert.Null(device.Device);
                Assert.NotNull(device.ErrorMessage);
            }
        }

        private void CreateDevices(DevicesDbContext dbContext)
        {
            for (int i = 0; i < 10; i++)
            {
                dbContext.Counters.Add(
                    new Counter()
                    {
                        SerialNumber = $"SN{i}",
                        //even-electricity   odd-water
                        Type = i % 2 == 0 ? Enums.CounterType.Electricity : Enums.CounterType.Water
                    }
                 ); 

            }
            dbContext.SaveChanges();
        }
    }
}
