using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<BookingDAO>
    {
        public void Configure(EntityTypeBuilder<BookingDAO> builder)
        {
            builder.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(sysutcdatetime())");

            builder.HasOne(d => d.BookingStatus)
                .WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Status");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_User");
        }
    }
}
