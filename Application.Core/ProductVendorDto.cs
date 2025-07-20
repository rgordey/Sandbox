// Application/ProductVendorDto.cs (new DTO)
namespace Application
{
    public class ProductVendorDto
    {
        public Guid ProductId { get; set; }
        public Guid VendorId { get; set; }
        public decimal VendorPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}