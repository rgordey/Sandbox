namespace Application.Core
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}


