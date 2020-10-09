using System.ComponentModel.DataAnnotations;

namespace SE.Service.Devices.Db
{
    public class Gateway : Device
    {
        [Required]
        public string IP { get; set; }
        public int Port { get; set; }

    }
}
