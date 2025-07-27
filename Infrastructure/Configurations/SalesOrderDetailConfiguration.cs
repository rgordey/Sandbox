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
            builder.HasKey(d => d.Id);
            builder.Property(d => d.UnitPrice).HasPrecision(18, 2);
            builder.Property(d => d.Status).HasConversion<string>();  // Store enum as string
            
            builder.HasOne(d => d.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(d => d.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(d => d.Product)
                   .WithMany()  // Assuming no reverse navigation on Product
                   .HasForeignKey(d => d.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.OrderId)
                   .HasDatabaseName("IX_OrderDetails_OrderId");
            builder.HasIndex(d => d.ProductId)
                   .HasDatabaseName("IX_OrderDetails_ProductId");
        }
    }
}