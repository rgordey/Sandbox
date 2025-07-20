// Application/PurchaseOrderDto.cs (new)
namespace Application
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PurchaseOrderDetailDto> OrderDetails { get; set; } = new List<PurchaseOrderDetailDto>();
    }
}