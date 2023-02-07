using FuelIn.Models.Auth;
using FuelIn.Models.StationData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models.FuelData
{
    public class fualDistributions
    {
        [Key]
        public int disID { get; set; }
        //Station Foreign Key
        public int staID { get; set; }
        public stations station { get; set; }
        //User (type=driver) Foreign Key
        [ForeignKey("User")]
        public int USER_ID { get; set; }
        public User User { get; set; }
        [Required]
        public DateTime distributionStartDate { get; set; }
        public DateTime expectedEndDate { get; set; }
        [Required]
        public int isDistributionStatus { get; set; }
        public int arrivalHours { get; set; }
        [Required]
        public int accepted { get; set; }
        public string disLocation { get; set; }

    }
}
