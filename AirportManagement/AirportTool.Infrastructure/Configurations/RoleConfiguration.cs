using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Auth.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public const string UserRoleId = "11111111-1111-1111-1111-111111111111";
        public const string StaffRoleId = "22222222-2222-2222-2222-222222222222";

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = UserRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "USER-CS"
                },
                new IdentityRole
                {
                    Id = StaffRoleId,
                    Name = "Staff",
                    NormalizedName = "STAFF",
                    ConcurrencyStamp = "STAFF-CS"
                }
            );
        }
    }
}
