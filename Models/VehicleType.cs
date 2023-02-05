using System.ComponentModel.DataAnnotations;
namespace FuelIn.Models
{
    public class VehicleType
    {
        [Key]
        public string VehTypeID { get; set; }
        public int WeeklyQuota { get; set; }
        public string VehType { get; set; }
    }
}
