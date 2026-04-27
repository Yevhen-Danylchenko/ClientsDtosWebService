using System.ComponentModel.DataAnnotations;

namespace ClientsDtosWebService.Models
{
    public class ClientFormModel
    {
        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter your password")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
    }
}
