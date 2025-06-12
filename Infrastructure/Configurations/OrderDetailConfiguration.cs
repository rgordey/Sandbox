using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(od => od.Id);
            builder.Property(od => od.ProductName).IsRequired().HasMaxLength(100);
            builder.Property(od => od.UnitPrice).HasPrecision(18, 2);
            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId);

            builder.HasIndex(od => od.OrderId)
                   .HasDatabaseName("IX_OrderDetails_OrderId");
        }
    }
}