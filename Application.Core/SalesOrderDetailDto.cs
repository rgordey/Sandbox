// Updated Application/OrderDetailDto.cs
using Domain;

namespace Application
{
    public sealed class SalesOrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public SalesOrderDetailStatus Status { get; set; } = SalesOrderDetailStatus.Pending;  // New: Detail-level status
        public string? ProductName { get; set; } // Optional for display, populated via query
    }
}