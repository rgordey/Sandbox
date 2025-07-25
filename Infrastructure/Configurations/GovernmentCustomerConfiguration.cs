using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class GovernmentCustomerConfiguration : IEntityTypeConfiguration<GovernmentCustomer>
    {
        public void Configure(EntityTypeBuilder<GovernmentCustomer> builder)
        {
            // Government-specific properties
            builder.Property(g => g.AgencyName).IsRequired().HasMaxLength(100);
            builder.Property(g => g.Department).IsRequired().HasMaxLength(50);
            builder.Property(g => g.IsFederal);
        }
    }
}