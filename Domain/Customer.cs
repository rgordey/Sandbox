using System.ComponentModel.DataAnnotations;

namespace Domain
{
    // Entity Models
    public abstract class Customer
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public Address? MailingAddress { get; set; }
        public Address? ShippingAddress { get; set; }
        public List<SalesOrder> Orders { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public sealed class ResidentialCustomer : Customer
    {
        private string firstName = null!;
        private string lastName = null!;

        public bool IsSeniorDiscountEligible { get; set; }
        [StringLength(25)]
        public string FirstName { get => firstName; set 
            {
                firstName = value;
                Name = $"{firstName} {lastName}";
            } }
        [StringLength(25)]
        public string LastName { get => lastName; set 
            {
                lastName = value;
                Name = $"{firstName} {lastName}";
            } }
        // Add more residential-specific properties...
    }

    public sealed class CorporateCustomer : Customer
    {
        [StringLength(100)]
        public string CompanyName { get; set; } = null!;
        public int EmployeeCount { get; set; }
        [StringLength(50)]
        public string Industry { get; set; } = null!;

        // Add more corporate-specific properties...
    }

    public sealed class GovernmentCustomer : Customer
    {
        [StringLength(100)]
        public string AgencyName { get; set; } = null!;

        [StringLength(50)]
        public string Department { get; set; } = null!;

        public bool IsFederal { get; set; }

        // Add more government-specific properties...
    }
}


