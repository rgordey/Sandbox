namespace Domain
{
    // Entity Models
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Address? MailingAddress { get; set; }
        public Address? ShippingAddress { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}


