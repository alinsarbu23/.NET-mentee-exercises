using AirportTool.Application.Auth;
using AirportTool.Application.Interfaces;
using AirportTool.Infrastructure.Auth;
using AirportTool.Infrastructure.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace AirportTool.Application.Services.Auth
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> userManager;
        private readonly JwtTokenService jwtTokenService;

        public AuthManager(UserManager<ApiUser> userManager, JwtTokenService jwtTokenService)
        {
            this.userManager = userManager;
            this.jwtTokenService = jwtTokenService;
        }

        public async Task<IEnumerable<string>> RegisterUserAsync(RegisterDto dto, CancellationToken ct)
        {
            var existing = await userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return new[] { "Email already registered." };

            var user = new ApiUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return result.Errors.Select(e => e.Description);

            await userManager.AddToRoleAsync(user, "User");

            return Array.Empty<string>();
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return null;

            var ok = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!ok)
                return null;

            var token = await jwtTokenService.CreateTokenAsync(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Token = token ?? ""
            };
        }
    }
}
