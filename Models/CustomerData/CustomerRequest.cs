using FuelIn.Models.StationData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models.CustomerData
{
    public class CustomerRequest
    {
        [Key]
        public int ReqId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int cusID { get; set; }
        public customers Customer { get; set; }

        public string Token { get; set; }
        [Required]
        public string ReqStatus { get; set; } //Status: Pending, Accepted, Rejected

        public DateTime ExpectedFillingTime { get; set; }
        [Required]
        public int RequestedQuota { get; set; }

        public int TotalPrice { get; set; }

    }
}
