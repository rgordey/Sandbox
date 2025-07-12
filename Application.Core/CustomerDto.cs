namespace Application.Core
{
    // DTOs
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}


