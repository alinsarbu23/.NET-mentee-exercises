using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Data.Configurations
{
    public class AirportConfiguration : IEntityTypeConfiguration<AirportDAO>
    {
        public void Configure(EntityTypeBuilder<AirportDAO> builder)
        {
            builder.Property(e => e.IATACode).IsFixedLength();

            builder.HasOne(d => d.Address)
                .WithMany(p => p.Airports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Airport_Address");
        }
    }
}
