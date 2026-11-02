using System.ComponentModel.DataAnnotations;

namespace MASH.Models
{
    public class Registration
    {
        [Required, MaxLength(100)]
        public string Username { get; set; }
        
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
