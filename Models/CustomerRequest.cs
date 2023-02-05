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
        public string StaId { get; set; }
        public StationModel Station { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CusId { get; set; }
        public Customer Customer { get; set; }  
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
