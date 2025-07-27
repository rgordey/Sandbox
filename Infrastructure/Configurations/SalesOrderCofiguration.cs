using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            builder.ToTable("SalesOrders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Property(o => o.Status).HasConversion<string>();  // New: Store enum as string
            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId);

            builder.Property(c => c.SequentialNumber)
            .HasDefaultValueSql("NEXT VALUE FOR SalesOrderNumberSequence");

            builder.Property(c => c.OrderNumber)
                .HasMaxLength(8)
                .HasComputedColumnSql("CONCAT('SO', RIGHT('000000' + CAST([SequentialNumber] AS VARCHAR(6)), 6))", stored: true);

            builder.HasIndex(o => o.CustomerId)
                   .HasDatabaseName("IX_Orders_CustomerId");
        }
    }
}