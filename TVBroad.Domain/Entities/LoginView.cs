using System.ComponentModel.DataAnnotations;

namespace TVBroad.Domain.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }  // Admin, Scheduler, Approver
    }
}
