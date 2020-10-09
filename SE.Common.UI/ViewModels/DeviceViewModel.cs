using System;
using System.ComponentModel.DataAnnotations;

namespace SE.Common.UI.ViewModels
{
    //This class has common properties for devices
    public class DeviceViewModel
    {
        //Uncomment for client validation 
        //[Required]
        [Display(Name = "Serial Number")]
        [MaxLength(10, ErrorMessage = "Serial Number cannot exceed 10 characters")]
        public String SerialNumber { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }
    }
}
