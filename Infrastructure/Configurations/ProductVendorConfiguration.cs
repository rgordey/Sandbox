// Infrastructure/Configurations/ProductVendorConfiguration.cs (new)
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ProductVendorConfiguration : IEntityTypeConfiguration<ProductVendor>
    {
        public void Configure(EntityTypeBuilder<ProductVendor> builder)
        {
            builder.HasKey(pv => new { pv.ProductId, pv.VendorId });
            builder.Property(pv => pv.VendorPrice).HasPrecision(18, 2);
            builder.Property(pv => pv.StockQuantity).IsRequired();

            builder.HasOne(pv => pv.Product)
                   .WithMany(p => p.ProductVendors)
                   .HasForeignKey(pv => pv.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pv => pv.Vendor)
                   .WithMany(v => v.ProductVendors)
                   .HasForeignKey(pv => pv.VendorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}