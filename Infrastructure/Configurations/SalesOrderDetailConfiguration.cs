using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SalesOrderDetailConfiguration : IEntityTypeConfiguration<SalesOrderDetail>
    {
        public void Configure(EntityTypeBuilder<SalesOrderDetail> builder)
        {
            builder.ToTable("SalesOrderDetails");
            builder.HasKey(od => od.Id);
            builder.Property(od => od.UnitPrice).HasPrecision(18, 2);
            builder.Property(od => od.Quantity).IsRequired();
            builder.Property(od => od.ProductId).IsRequired();

            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Cascade delete with Order

            builder.HasOne(od => od.Product)
                   .WithMany() // No navigation back to OrderDetails
                   .HasForeignKey(od => od.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Product if referenced

            builder.HasIndex(od => od.OrderId)
                   .HasDatabaseName("IX_OrderDetails_OrderId");
            builder.HasIndex(od => od.ProductId)
                   .HasDatabaseName("IX_OrderDetails_ProductId");
        }
    }
}