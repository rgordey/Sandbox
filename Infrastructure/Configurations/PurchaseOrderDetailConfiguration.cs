// Infrastructure/Configurations/PurchaseOrderDetailConfiguration.cs (new)
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PurchaseOrderDetailConfiguration : IEntityTypeConfiguration<PurchaseOrderDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderDetail> builder)
        {
            builder.ToTable("PurchaseOrderDetails");
            builder.HasKey(od => od.Id);
            builder.Property(od => od.UnitPrice).HasPrecision(18, 2);
            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId);

            builder.HasIndex(od => od.OrderId)
                   .HasDatabaseName("IX_PurchaseOrderDetails_OrderId");
        }
    }
}