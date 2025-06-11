namespace Application.Core
{
    // DTOs
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}


