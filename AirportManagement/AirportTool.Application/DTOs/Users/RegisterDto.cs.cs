using System.ComponentModel.DataAnnotations;

namespace AirportTool.Infrastructure.DTOs.Auth
{
    public class RegisterDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";
    }
}
