using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Data.Configurations
{
    public class GateConfiguration : IEntityTypeConfiguration<GateDAO>
    {
        public void Configure(EntityTypeBuilder<GateDAO> builder)
        {
            builder.HasOne(d => d.Airport)
                .WithMany(p => p.Gates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gate_Airport");
        }
    }
}
