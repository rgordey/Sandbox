// Update Domain/Product.cs (many vendors)
namespace Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal BasePrice { get; set; } // Optional base price if needed

        public List<ProductVendor> ProductVendors { get; set; } = new(); // Many vendors
    }
}