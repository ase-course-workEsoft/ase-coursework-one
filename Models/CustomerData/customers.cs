using FuelIn.Models.Auth;
using FuelIn.Models.StationData;
using System.ComponentModel.DataAnnotations;

namespace FuelIn.Models.CustomerData
{
    public class customers
    {
        [Key]
        public int cusID { get; set; }
        //Station Foreign Key
        public int staID { get; set; }
        public stations station { get; set; }
        //Vehicle Type Foreign Key
        public int vehTypeID { get; set; }
        public vehicleTypes vehicleType { get; set; }
        //User Foreign Key
        public int USER_ID { get; set; }
        public User User { get; set; }
        [Required]
        public string cusName { get; set; }
        [Required]
        public string cusNIC { get; set; }
        [Required]
        public string vehicleRegNum { get; set; }
        [Required]
        public int avaWeeklyQuota { get; set; }
    }
}
