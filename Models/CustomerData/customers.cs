using FuelIn.Models.Auth;
using FuelIn.Models.StationData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models.CustomerData
{
    public class customers
    {
        [Key]
        public int cusID { get; set; }
        //Station Foreign Key
        [ForeignKey("station")]
        public int staID { get; set; }
        public stations station { get; set; }

        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage = "PLease select your vehicle type")]
        public string StaDistrict { get; set; }

        //Vehicle Type Foreign Key
        [ForeignKey("vehicleTypes")]
        public int vehTypeID { get; set; }
        public vehicleTypes vehicleTypes { get; set; }

        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage = "PLease select your vehicle type")]
        public string VehType { get; set; }

        //User Foreign Key
        [ForeignKey("User")]
        public int USER_ID { get; set; }
        public User User { get; set; }

        [Required]
        public string cusName { get; set; }
        [Required]
        public string cusNIC { get; set; }
        public string? cusEmail { get; set; }
        [Required]
        public string vehicleRegNum { get; set; }
        [Required]
        public int avaWeeklyQuota { get; set; }
    }
}
