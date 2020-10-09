namespace SE.Service.Devices.Profile
{

    public class DeviceProfile : AutoMapper.Profile
    {
        //ctor defines mappings between classes. Devices are injected in the mapper. All classes derived from Automapper.Profile
        public DeviceProfile()
        {
            CreateMap<Db.Counter, Models.Counter>();
            CreateMap<Models.Counter, Db.Counter>();
            CreateMap<Db.Gateway, Models.Gateway>();
            CreateMap<Models.Gateway, Db.Gateway>();
        }
    }
}
