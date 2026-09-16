using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Data.Configurations
{
    public class FlightScheduleConfiguration : IEntityTypeConfiguration<FlightScheduleDAO>
    {
        public void Configure(EntityTypeBuilder<FlightScheduleDAO> builder)
        {
            builder.HasOne(d => d.AssignedAircraft)
                .WithMany(p => p.FlightSchedules)
                .HasConstraintName("FK_FlightSchedule_AssignedAircraft");

            builder.HasOne(d => d.Flight)
                .WithMany(p => p.FlightSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightSchedule_Flight");

            builder.HasOne(d => d.FlightStatus)
                .WithMany(p => p.FlightSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightSchedule_FlightStatus");

            builder.HasOne(d => d.Gate)
                .WithMany(p => p.FlightSchedules)
                .HasConstraintName("FK_FlightSchedule_Gate");

        }
    }
}
