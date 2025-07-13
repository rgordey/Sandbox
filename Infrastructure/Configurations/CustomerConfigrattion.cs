using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(100);

            builder.HasIndex(c => c.Email)
                   .IsUnique()
                   .HasDatabaseName("IX_Customers_Email");

            // Configure MailingAddress as an owned entity (nullable)
            builder.OwnsOne(c => c.MailingAddress, ma =>
            {
                ma.Property(a => a.Street).HasColumnName("MailingStreet").HasMaxLength(100);
                ma.Property(a => a.City).HasColumnName("MailingCity").HasMaxLength(50);
                ma.Property(a => a.State).HasColumnName("MailingState").HasMaxLength(50);
                ma.Property(a => a.ZipCode).HasColumnName("MailingZipCode").HasMaxLength(20);
            });

            // Configure ShippingAddress as an owned entity (nullable)
            builder.OwnsOne(c => c.ShippingAddress, sa =>
            {
                sa.Property(a => a.Street).HasColumnName("ShippingStreet").HasMaxLength(100);
                sa.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(50);
                sa.Property(a => a.State).HasColumnName("ShippingState").HasMaxLength(50);
                sa.Property(a => a.ZipCode).HasColumnName("ShippingZipCode").HasMaxLength(20);
            });
        }
    }
}