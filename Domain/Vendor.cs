// Update Domain/Vendor.cs (many products)
namespace Domain
{
    public sealed class Vendor
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public int SequentialNumber { get; set; }
        public string? VendorNumber { get; set; }
        public Address? Address { get; set; }

        public List<PurchaseOrder> PurchaseOrders { get; set; } = new();

        public List<ProductVendor> ProductVendors { get; set; } = new(); // Many products
    }
}