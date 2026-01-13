using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Data.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<FlightDAO>
    {
        public void Configure(EntityTypeBuilder<FlightDAO> builder)
        {
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(d => d.Airline)
                .WithMany(p => p.Flights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_Airline");

            builder.HasOne(d => d.DefaultAircraft)
                .WithMany(p => p.Flights)
                .HasConstraintName("FK_Flight_DefaultAircraft");

            builder.HasOne(d => d.DestinationAirport)
                .WithMany(p => p.FlightDestinationAirports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_DestinationAirport");

            builder.HasOne(d => d.OriginAirport)
                .WithMany(p => p.FlightOriginAirports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_OriginAirport");

        }
    }
}
