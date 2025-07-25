using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ResidentialCustomerConfiguration : IEntityTypeConfiguration<ResidentialCustomer>
    {
        public void Configure(EntityTypeBuilder<ResidentialCustomer> builder)
        {
            // Residential-specific properties
            builder.Property(r => r.FirstName).IsRequired().HasMaxLength(25);
            builder.Property(r => r.LastName).IsRequired().HasMaxLength(25);
            builder.Property(r => r.IsSeniorDiscountEligible);
        }
    }
}