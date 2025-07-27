// Application/VendorDto.cs
namespace Application
{
    public sealed class VendorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? VendorNumber { get; set; }
        public string ContactEmail { get; set; } = null!;
        public AddressDto Address { get; set; } = null!;
        public decimal VendorPrice { get; set; }  // Vendor-specific price for the product (cost price)
        public int StockQuantity { get; set; }
        public List<PurchaseOrderDto> PurchaseOrders { get; set; } = new();        
    }
}