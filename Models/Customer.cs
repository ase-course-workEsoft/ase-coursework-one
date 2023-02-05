using FuelIn.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models
{
    public class Customer
    {
        [Key]
        public int CusId { get; set; }

        [Required(ErrorMessage = "Station district is required")]
        [ForeignKey("Station")]
        public string StaId { get; set; }
        public StationModel Station { get; set; }

        [Required(ErrorMessage = "PLease select your vehicle type")]
        [ForeignKey("VehicleType")]
        public string VehTypeID { get; set; }
        public VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string CusName { get; set; }

        [Required]
        [ForeignKey("User")]
        public int USER_ID { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "NIC is required")]
        public string CusNIC { get; set; }
        [Required(ErrorMessage = "Enter your vehicle registration number")]
        public string VehicleRegNum { get; set; }
        public int AvaWeeklyQuota { get; set; }
    }
}
