// Application/PurchaseOrderDetailDto.cs (new)
namespace Application
{
    public class PurchaseOrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; }
    }
}