using System.ComponentModel.DataAnnotations;

namespace FuelIn.Models.CustomerData
{
    public class vehicleTypes
    {
        [Key]
        public int vehTypeID { get; set; }
        [Required]
        public int weeklyQuota { get; set; }
        [Required]
        public string vehType { get; set; }

        //Give ID for Consumer
        public ICollection<customers> customers { get; set; }
    }
}
