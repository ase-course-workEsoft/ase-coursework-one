using System.ComponentModel.DataAnnotations;

namespace FuelIn.Models.Auth
{
    public class User
    {
        [Key]
        public int USER_ID { get; set; }
        [Required]
        public string USERNAME { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        [Required]
        public string PRIVILEGE_TYPE { get; set; } //Hardcoded => Super admin - "SUPER_ADMIN", Station Manager = "MANAGER", Driver = "DRIVER", Consumer = "CONSUMER"
        [Required] 
        public string USER_STATUS { get; set; } //Hardcoded => Active = "ACT", Inactive = "INA". INA records are considered to be deleted and unused records.
    }
}
