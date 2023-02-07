using FuelIn.Models.CustomerData;
using FuelIn.Models.StationData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models
{
    public class CustomerRequest
    {
        [Key]
        public int ReqId { get; set; }
        [Required]
        [ForeignKey("Station")]
        public int StaId { get; set; }
        public stations Station { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int cusID { get; set; }
        public customers Customer { get; set; }  
        [Required]
        public int IsIdUsed { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public int ReqStatus { get; set; }
        [Required]
        public DateTime ExpectedFillingTime { get; set; }
        [Required]
        public int RequestedQuota { get; set; }
        [Required]
        public int TotalPrice { get; set; }

    }
}
