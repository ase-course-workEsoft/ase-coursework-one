using System;
using System.ComponentModel.DataAnnotations;

namespace FuelIn.Models
{
    public class StationModel
    {
        [Key]
        public int StaId { get; set; }
        [Required]
        public string StaDistrict { get; set; }
        [Required]
        public int TotalFualQuota { get; set; }
        [Required]
        public int AvaFualQuota { get; set; }
        [Required]
        public DateTime NextFillingDate { get; set; }
    }
}
