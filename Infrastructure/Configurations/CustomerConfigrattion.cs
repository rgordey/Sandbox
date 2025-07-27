using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);

        // Common properties
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(100);  // Assuming max length; adjust if needed
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(50);  // Assuming max length; adjust if needed
        builder.Property(c => c.CreatedAt).IsRequired();

        builder.Property(c => c.SequentialNumber)
            .HasDefaultValueSql("NEXT VALUE FOR CustomerNumberSequence");

        builder.Property(c => c.CustomerNumber)
            .HasMaxLength(8)
            .HasComputedColumnSql("CONCAT('CUS', RIGHT('00000' + CAST([SequentialNumber] AS VARCHAR(5)), 5))", stored: true);

        // Unique index on Email
        builder.HasIndex(c => c.Email)
               .IsUnique()
               .HasDatabaseName("IX_Customers_Email");

        // Configure MailingAddress as an owned entity (nullable)
        builder.OwnsOne(c => c.MailingAddress, ma =>
        {
            ma.Property(a => a.Line1).HasColumnName("MailingStreet").HasMaxLength(100);
            ma.Property(a => a.City).HasColumnName("MailingCity").HasMaxLength(50);
            ma.Property(a => a.State).HasColumnName("MailingState").HasMaxLength(50);
            ma.Property(a => a.ZipCode).HasColumnName("MailingZipCode").HasMaxLength(20);
        });

        // Configure ShippingAddress as an owned entity (nullable)
        builder.OwnsOne(c => c.ShippingAddress, sa =>
        {
            sa.Property(a => a.Line1).HasColumnName("ShippingStreet").HasMaxLength(100);
            sa.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(50);
            sa.Property(a => a.State).HasColumnName("ShippingState").HasMaxLength(50);
            sa.Property(a => a.ZipCode).HasColumnName("ShippingZipCode").HasMaxLength(20);
        });

        // Relationship to Orders (assuming SalesOrder config elsewhere; configure if needed)
        builder.HasMany(c => c.Orders)
               .WithOne(o => o.Customer)  // Assuming SalesOrder has Customer navigation
               .HasForeignKey(o => o.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);  // Adjust cascade as needed
    }
}