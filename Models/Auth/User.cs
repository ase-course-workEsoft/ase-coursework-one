using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelIn.Models.Auth
{
    public class User
    {
        [Key]
        public int USER_ID { get; set; }
        [Required(ErrorMessage = "User name is required")]
        public string USERNAME { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string PASSWORD { get; set; }
        [NotMapped] // Does not effect with your database
        [Compare("PASSWORD")]
        public string PASSWORD_CON { get; set; }
        [Required]
        public string PRIVILEGE_TYPE { get; set; } //Hardcoded => Super admin - "SUPER_ADMIN", Station Manager = "MANAGER", Driver = "DRIVER", Consumer = "CONSUMER"
        [Required] 
        public string USER_STATUS { get; set; } //Hardcoded => Active = "ACT", Inactive = "INA". INA records are considered to be deleted and unused records.
    }
}
