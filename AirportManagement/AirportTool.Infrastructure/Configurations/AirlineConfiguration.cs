using AirportTool.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirportTool.Infrastructure.Data.Configurations
{
    public class AirlineConfiguration : IEntityTypeConfiguration<AirlineDAO>
    {
        public void Configure(EntityTypeBuilder<AirlineDAO> builder)
        {
            builder.Property(e => e.IATACode).IsFixedLength();
        }
    }
}
