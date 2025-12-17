using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure;

public partial class AirportDbContext : DbContext
{
    public AirportDbContext()
    {
    }

    public AirportDbContext(DbContextOptions<AirportDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddressDAO> Addresses { get; set; }

    public virtual DbSet<AircraftDAO> Aircraft { get; set; }

    public virtual DbSet<AirlineDAO> Airlines { get; set; }

    public virtual DbSet<AirportDAO> Airports { get; set; }

    public virtual DbSet<BookingDAO> Bookings { get; set; }

    public virtual DbSet<BookingStatusDAO> BookingStatuses { get; set; }

    public virtual DbSet<FlightDAO> Flights { get; set; }

    public virtual DbSet<FlightScheduleDAO> FlightSchedules { get; set; }

    public virtual DbSet<FlightStatusDAO> FlightStatuses { get; set; }

    public virtual DbSet<GateDAO> Gates { get; set; }

    public virtual DbSet<TicketDAO> Tickets { get; set; }

    public virtual DbSet<UserDAO> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AirlineDAO>(entity =>
        {
            entity.Property(e => e.IATACode).IsFixedLength();
        });

        modelBuilder.Entity<AirportDAO>(entity =>
        {
            entity.Property(e => e.IATACode).IsFixedLength();

            entity.HasOne(d => d.Address).WithMany(p => p.Airports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Airport_Address");
        });

        modelBuilder.Entity<BookingDAO>(entity =>
        {
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.BookingStatus).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Status");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_User");
        });

        modelBuilder.Entity<FlightDAO>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Airline).WithMany(p => p.Flights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_Airline");

            entity.HasOne(d => d.DefaultAircraft).WithMany(p => p.Flights).HasConstraintName("FK_Flight_DefaultAircraft");

            entity.HasOne(d => d.DestinationAirport).WithMany(p => p.FlightDestinationAirports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_DestinationAirport");

            entity.HasOne(d => d.OriginAirport).WithMany(p => p.FlightOriginAirports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_OriginAirport");
        });

        modelBuilder.Entity<FlightScheduleDAO>(entity =>
        {
            entity.HasOne(d => d.AssignedAircraft).WithMany(p => p.FlightSchedules).HasConstraintName("FK_FlightSchedule_AssignedAircraft");

            entity.HasOne(d => d.Flight).WithMany(p => p.FlightSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightSchedule_Flight");

            entity.HasOne(d => d.FlightStatus).WithMany(p => p.FlightSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightSchedule_FlightStatus");

            entity.HasOne(d => d.Gate).WithMany(p => p.FlightSchedules).HasConstraintName("FK_FlightSchedule_Gate");
        });

        modelBuilder.Entity<GateDAO>(entity =>
        {
            entity.HasOne(d => d.Airport).WithMany(p => p.Gates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gate_Airport");
        });

        modelBuilder.Entity<TicketDAO>(entity =>
        {
            entity.Property(e => e.Currency).IsFixedLength();

            entity.HasOne(d => d.Booking).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Booking");

            entity.HasOne(d => d.FlightSchedule).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_FlightSchedule");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
