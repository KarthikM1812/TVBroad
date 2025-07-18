using System.ComponentModel.DataAnnotations;

namespace TVBroad.Web.Models
{
    public class RegisterView
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Scheduler or Approver
    }
}
