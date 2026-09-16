using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<TicketDAO>
    {
        public void Configure(EntityTypeBuilder<TicketDAO> builder)
        {
            builder.Property(e => e.Currency).IsFixedLength();

            builder.HasOne(d => d.Booking)
                .WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Booking");

            builder.HasOne(d => d.FlightSchedule)
                .WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_FlightSchedule");
        }
    }
}
