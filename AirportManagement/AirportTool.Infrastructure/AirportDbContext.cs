using AirportTool.Infrastructure.Auth;
using AirportTool.Infrastructure.Auth.Configurations;
using AirportTool.Infrastructure.Data.Configurations;
using AirportTool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace AirportTool.Infrastructure;

public partial class AirportDbContext : IdentityDbContext<ApiUser>
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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

        modelBuilder.ApplyConfiguration(new AirlineConfiguration());
        modelBuilder.ApplyConfiguration(new AirportConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new FlightConfiguration());
        modelBuilder.ApplyConfiguration(new FlightScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new GateConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());

       

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
