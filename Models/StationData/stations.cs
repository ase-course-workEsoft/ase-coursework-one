using FuelIn.Models.CustomerData;
using System.ComponentModel.DataAnnotations;

namespace FuelIn.Models.StationData
{
    public class stations
    {
        [Key]
        public int staID { get; set; }
        [Required]
        public string staDistrict { get; set; }
        [Required]
        public int totalFualQuota { get; set; }
        [Required]
        public int avaFualQuota { get; set; }
        [Required]
        public DateTime nextFillingDate { get; set; }

        //Give ID for Consumer
        public ICollection<customers> customers { get; set; }
    }
}
