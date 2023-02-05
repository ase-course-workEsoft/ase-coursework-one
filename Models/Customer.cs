using FuelIn.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models
{
    public class Customer
    {
        [Key]
        public int CusId { get; set; }
        [Required]
        [ForeignKey("Station")]
        public int StaId { get; set; }
        public StationModel Station { get; set; }

        [Required]
        [ForeignKey("VehicleType")]
        public int VehTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        [Required]
        public string CusName { get; set; }

        [Required]
        [ForeignKey("User")]
        public int USER_ID { get; set; }
        public User User { get; set; }

        [Required]
        public string CusNIC { get; set; }
        [Required]
        public string VehicleRegNum { get; set; }
        [Required]
        public int AvaWeeklyQuota { get; set; }
    }
}
