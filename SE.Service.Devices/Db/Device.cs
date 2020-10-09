using SE.Service.Devices.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SE.Service.Devices.Db
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        public String SerialNumber { get; set; }

        public BrandType Brand { get; set; }

        public ModelType Model { get; set; }
    }
}
