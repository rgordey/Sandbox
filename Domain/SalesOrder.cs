namespace Domain
{
    public sealed class SalesOrder
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int SequentialNumber { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.Placed;  // New: Order-level status
        public Customer? Customer { get; set; }
        public List<SalesOrderDetail> OrderDetails { get; set; } = new();
    }
}