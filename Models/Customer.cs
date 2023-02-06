using FuelIn.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models
{
    public class Customer
    {
        [Key]
        public int CusId { get; set; }
        [ForeignKey("Station")]
        public int StaId { get; set; }
        public StationModel Station { get; set; }

        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage = "PLease select your vehicle type")]
        public string StaDistrict { get; set; }

        [ForeignKey("VehicleType")]
        public int VehTypeId { get; set; }

        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage = "PLease select your vehicle type")]
        public string VehType { get; set; }

        public VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string CusName { get; set; }

        [ForeignKey("User")]
        public int USER_ID { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "NIC is required")]
        public string CusNIC { get; set; }
        [Required(ErrorMessage = "Enter your vehicle registration number")]
        [Remote("IsExist", "Place", ErrorMessage = "Number exist!")]
        public string VehicleRegNum { get; set; }

        public int AvaWeeklyQuota { get; set; }
    }
}
