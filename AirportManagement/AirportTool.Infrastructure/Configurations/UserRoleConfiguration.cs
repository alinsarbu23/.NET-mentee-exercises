using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Auth.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = UserConfiguration.DemoUserId,
                    RoleId = RoleConfiguration.UserRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = UserConfiguration.DemoStaffId,
                    RoleId = RoleConfiguration.StaffRoleId
                }
            );
        }
    }
}
