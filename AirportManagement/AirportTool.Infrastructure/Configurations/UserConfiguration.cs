using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Auth.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApiUser>
    {
        public const string DemoUserId = "33333333-3333-3333-3333-333333333333";
        public const string DemoStaffId = "44444444-4444-4444-4444-444444444444";

        public void Configure(EntityTypeBuilder<ApiUser> builder)
        {
            var hasher = new PasswordHasher<ApiUser>();

            var userEmail = "alin@gmail.com";
            var staffEmail = "admin@gmail.com";

            var demoUser = new ApiUser
            {
                Id = DemoUserId,
                UserName = "alin",
                NormalizedUserName = userEmail.ToUpperInvariant(),
                Email = userEmail,
                NormalizedEmail = userEmail.ToUpperInvariant(),
                EmailConfirmed = true,
                FirstName = "Demo",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };
            demoUser.PasswordHash = hasher.HashPassword(demoUser, "alin1234!");

            var demoStaff = new ApiUser
            {
                Id = DemoStaffId,
                UserName = "admin",
                NormalizedUserName = staffEmail.ToUpperInvariant(),
                Email = staffEmail,
                NormalizedEmail = staffEmail.ToUpperInvariant(),
                EmailConfirmed = true,
                FirstName = "Demo",
                LastName = "Staff",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };
            demoStaff.PasswordHash = hasher.HashPassword(demoStaff, "admin1234!");

            builder.HasData(demoUser, demoStaff);
        }
    }
}
