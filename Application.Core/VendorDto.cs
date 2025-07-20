// Application/VendorDto.cs
namespace Application
{
    public class VendorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public AddressDto? Address { get; set; }
        public List<PurchaseOrderDto> PurchaseOrders { get; set; } = new();
        public decimal VendorPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}