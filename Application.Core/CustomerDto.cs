namespace Application
{
    // DTOs
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public AddressDto? MailingAddress { get; set; }
        public AddressDto? ShippingAddress { get; set; }
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}


