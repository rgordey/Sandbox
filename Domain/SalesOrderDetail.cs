// Updated Domain/OrderDetail.cs
namespace Domain
{
    public sealed class SalesOrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public SalesOrderDetailStatus Status { get; set; } = SalesOrderDetailStatus.Pending;  // New: Detail-level status
        public SalesOrder? Order { get; set; }
        public Product? Product { get; set; }
    }
}