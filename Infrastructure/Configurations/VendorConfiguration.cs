using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("Vendors");
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
            builder.Property(v => v.ContactEmail).HasMaxLength(100);

            // Define Address as an owned type
            builder.OwnsOne(v => v.Address, a =>
            {
                a.Property(p => p.Line1).HasMaxLength(200).HasColumnName("Address_Street");
                a.Property(p => p.City).HasMaxLength(100).HasColumnName("Address_City");
                a.Property(p => p.State).HasMaxLength(50).HasColumnName("Address_State");
                a.Property(p => p.ZipCode).HasMaxLength(20).HasColumnName("Address_ZipCode");
            });

            builder.HasIndex(v => v.Name)
                   .HasDatabaseName("IX_Vendors_Name");
            builder.HasIndex(v => v.ContactEmail)
                   .HasDatabaseName("IX_Vendors_ContactEmail");
        }
    }
}