using System.ComponentModel.DataAnnotations;

namespace AirportTool.Infrastructure.DTOs.Auth
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
