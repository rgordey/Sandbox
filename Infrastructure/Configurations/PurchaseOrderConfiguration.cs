// Infrastructure/Configurations/PurchaseOrderConfiguration.cs (new)
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.HasOne(o => o.Vendor)
                   .WithMany(v => v.PurchaseOrders)
                   .HasForeignKey(o => o.VendorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.VendorId)
                   .HasDatabaseName("IX_PurchaseOrders_VendorId");
        }
    }
}