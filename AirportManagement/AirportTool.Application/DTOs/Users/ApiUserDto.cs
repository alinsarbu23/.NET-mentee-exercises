using AirportTool.Infrastructure.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace AirportTool.WebApi.Models.Users
{
    public class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
