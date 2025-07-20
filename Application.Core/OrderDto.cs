// Application/OrderDto.cs (updated to include CustomerFullName)
namespace Application
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerFullName { get; set; } = string.Empty; // Computed in mapping
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
}