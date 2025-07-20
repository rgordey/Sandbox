namespace Application
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal BasePrice { get; set; } // Optional base price, if applicable
        public List<VendorDto> Vendors { get; set; } = new(); // List of vendors supplying this product
    }
}