using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Service.Devices.Db;
using SE.Service.Devices.Enums;
using SE.Service.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Service.Devices.Providers
{
    /// <summary> Class <c>DevicesProvider</c>
    /// This class will help as business layer to interact with DbCOntext 
    /// </summary>
    public class DevicesProvider : IDevicesProvider
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DevicesProvider> _logger;
        private readonly DevicesDbContext _dbContext;
        private readonly IMapper _mapper;


        public DevicesProvider(DevicesDbContext dbContext, ILogger<DevicesProvider> logger, IMapper mapper)
        {

            //_serviceProvider = serviceProvider; This is .net core native service provider, useful when need an object anywhere
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;

            SeedData();

        }

        //Typical seedData method to populate in-Memory db,data will be kept in memory for the WebAPI life cycle
        private void SeedData()
        {


            if (!_dbContext.Counters.Any())
            {
                _dbContext.Counters.Add(new Db.Counter()
                {
                    Id = 1,
                    SerialNumber = "CW1",
                    Model = Enums.ModelType.MODEL1,
                    Brand = Enums.BrandType.BRAND1,
                    Type = Enums.CounterType.Water
                });

                _dbContext.Counters.Add(new Db.Counter()
                {
                    Id = 2,
                    SerialNumber = "CE1",
                    Model = Enums.ModelType.MODEL1,
                    Brand = Enums.BrandType.BRAND2,
                    Type = Enums.CounterType.Electricity
                });

                _dbContext.Counters.Add(new Db.Counter()
                {
                    Id = 3,
                    SerialNumber = "CE2",
                    Model = Enums.ModelType.MODEL2,
                    Brand = Enums.BrandType.BRAND1,
                    Type = Enums.CounterType.Electricity
                });


            }

            if (!_dbContext.Gateways.Any())
            {
                _dbContext.Gateways.Add(new Db.Gateway()
                {
                    Id = 4,
                    SerialNumber = "G1",
                    Model = Enums.ModelType.MODEL2,
                    Brand = Enums.BrandType.BRAND1,
                    IP = "192.168.0.1",
                    Port = 80
                });

                _dbContext.Gateways.Add(new Db.Gateway()
                {
                    Id = 5,
                    SerialNumber = "G2",
                    Model = Enums.ModelType.MODEL2,
                    Brand = Enums.BrandType.BRAND2,
                    IP = "192.168.0.2",
                    Port = 81
                });

            }
            _dbContext.SaveChanges();

        }


        /// <summary>This method insert a counter in-memory DB. We do Unique constranint and validation</summary>
        /// <param><c>model</c> is the object to be inserted.
        /// </param>
        /// <returns>Tuple(int,Counter,string) 
        /// Id returns id PK returned from insertion. If any error or validation then return -1
        /// returns basically same counter model or null if any problem
        /// ErrorMessage returns a string with a message, if ID is -1 or String.Empty if no problem with insertion
        /// </returns>
        public async Task<(int Id, Models.Counter Device, string ErrorMessage)> InsertCounterAsync(Models.Counter model)
        {
            try
            {
                //It´s very annoying that DataAnnotations for table fields does not work in-memory
                //SerialNumber has [Required] but it´s not working, so I manually check it out and act accordingly
                if (!CheckValidDevice(model)) 
                    return (-1, null, "Serial Number can not be empty or null");

                //In-memory DB does not allow constraints, so manually constrained
                //get counter with same serial number if exists
                var existingCounter = _dbContext.Counters.FirstOrDefault(c => c.SerialNumber == model.SerialNumber
                                                                            && c.Model == model.Model
                                                                            && c.Brand == model.Brand
                                                                            && c.Type == model.Type);
                //if not exists then go and create
                if (existingCounter == null)
                {
                    var entity = _mapper.Map<Models.Counter, Db.Counter>(model);
                    await _dbContext.Counters.AddAsync(entity);
                    _dbContext.SaveChanges();
                    var counterModel = _mapper.Map<Db.Counter, Models.Counter>(entity);
                    return (entity.Id, counterModel, String.Empty);
                }
                return (-1, null, "Counter device already exists");

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (-1, null, ex.Message);
            }
        }


        /// <summary>This method insert a gateway in-memory DB. We do Unique constranint and validation</summary>
        /// <param><c>model</c> is the object to be compared to the current object.
        /// </param>
        /// <returns>Tuple(int,Counter,string) 
        /// Id returns id PK returned from insertion. If any error or validation then return -1
        /// returns basically same counter model or null if any problem
        /// ErrorMessage returns a string with a message, if ID is -1 or String.Empty if no problem with insertion
        /// </returns>
        public async Task<(int Id, Models.Gateway Device, string ErrorMessage)> InsertGatewayAsync(Models.Gateway model)
        {
            try
            {
                //It´s very annoying that DataAnnotations for table fields does not work in-memory
                //SerialNumber has [Required] but it´s not working, so I manually check it out and act accordingly
                if (!CheckValidDevice(model))
                    return (-1, null, "Serial Number or IP Address can not be empty or null");

                //In-memory DB does not allow constraints, so manually constrained
                //get gateway with same serial number,model,brand
                var existingGateway = _dbContext.Gateways.FirstOrDefault(c => c.SerialNumber == model.SerialNumber 
                                                                            && c.Model == model.Model
                                                                            && c.Brand == model.Brand);
                //if not exists then go and create
                if (existingGateway == null)
                {
                    var entity = _mapper.Map<Models.Gateway, Db.Gateway>(model);
                    await _dbContext.Gateways.AddAsync(entity);
                    _dbContext.SaveChanges();
                    var gatewayModel = _mapper.Map<Db.Gateway, Models.Gateway>(entity);
                    return (entity.Id, gatewayModel, String.Empty);
                }
                return (-1, null, "Gateway device already exists");


            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (-1, null, ex.Message);
            }
        }

        /// <summary>This method retrieves all devices from DB</summary>
        /// <returns>Tuple(bool,IEnumerable<IDevice>,string) 
        /// IsSuccess returns true if everything goes fine
        /// Devices returns basically a list of Devices(Counters and Gateway) under IDevice interface look
        /// ErrorMessage returns a string with a message, if it´´s not success
        /// </returns>
        public async Task<(bool IsSuccess, IEnumerable<IDevice> Devices, string ErrorMessage)> GetDevicesAsync()
        {
            List<IDevice> devices = new List<IDevice>();
            try
            {

                var counters = await _dbContext.Counters.ToListAsync();
                var counterModels = _mapper.Map<IEnumerable<Db.Counter>, IEnumerable<Models.Counter>>(counters);

                var gateways = await _dbContext.Gateways.ToListAsync();
                var gatewayModels = _mapper.Map<IEnumerable<Db.Gateway>, IEnumerable<Models.Gateway>>(gateways);

                devices.AddRange(counterModels);
                devices.AddRange(gatewayModels);

                if (devices != null && devices.Any())
                {
                    return (true, devices, null);
                }

                return (false, null, "No Devices found");

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
                throw;
            }
        }

        public IEnumerable<string> GetBrands() => Enum.GetNames(typeof(BrandType));

        public IEnumerable<string> GetModels() => Enum.GetNames(typeof(ModelType));

        //use IDevice interface to check vaidation
        private bool CheckValidDevice(IDevice device)
        {
            return device.IsValidDevice() ? true : false;
        }
    }
}
