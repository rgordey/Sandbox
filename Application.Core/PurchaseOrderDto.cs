// Application/PurchaseOrderDto.cs
namespace Application
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? VendorName { get; set; } // For display
        public List<PurchaseOrderDetailDto> OrderDetails { get; set; } = new();
    }
}