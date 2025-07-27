// Application/OrderDto.cs (updated to include CustomerFullName)
using Domain;

namespace Application
{
    public sealed class SalesOrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderNumber { get; set; }
        public int SequentialNumber { get; set; }        
        public decimal TotalAmount { get; set; }
        public string CustomerFullName { get; set; } = string.Empty; // Computed in mapping
        public List<SalesOrderDetailDto> OrderDetails { get; set; } = new List<SalesOrderDetailDto>();
        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.Placed;  // New: Order-level status
    }
}