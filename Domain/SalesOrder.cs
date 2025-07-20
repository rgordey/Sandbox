namespace Domain
{
    public class SalesOrder
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Customer? Customer { get; set; } 
        public List<SalesOrderDetail> OrderDetails { get; set; } = new();
    }
}


