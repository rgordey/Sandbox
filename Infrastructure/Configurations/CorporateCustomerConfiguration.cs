using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CorporateCustomerConfiguration : IEntityTypeConfiguration<CorporateCustomer>
    {
        public void Configure(EntityTypeBuilder<CorporateCustomer> builder)
        {
            // Corporate-specific properties
            builder.Property(c => c.CompanyName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.EmployeeCount).IsRequired();  // int, defaults to required
            builder.Property(c => c.Industry).IsRequired().HasMaxLength(50);
        }
    }
}