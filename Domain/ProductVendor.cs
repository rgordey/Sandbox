// Domain/ProductVendor.cs (new junction entity)
namespace Domain
{
    public sealed class ProductVendor
    {
        public Guid ProductId { get; set; }
        public Guid VendorId { get; set; }
        public decimal VendorPrice { get; set; } // Vendor-specific price
        public int StockQuantity { get; set; } = 0; // Optional: Vendor-specific stock

        public Product? Product { get; set; }
        public Vendor? Vendor { get; set; }
    }
}