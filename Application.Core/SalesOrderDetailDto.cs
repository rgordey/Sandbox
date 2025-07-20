// Updated Application/OrderDetailDto.cs
namespace Application
{
    public class SalesOrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; } // Optional for display, populated via query
    }
}