// Update Domain/Vendor.cs (many products)
namespace Domain
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public Address? Address { get; set; }

        public List<PurchaseOrder> PurchaseOrders { get; set; } = new();

        public List<ProductVendor> ProductVendors { get; set; } = new(); // Many products
    }
}