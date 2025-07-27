namespace Application
{
    // DTOs
    public sealed class CustomerMetaDto 
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? CustomerNumber { get; set; }
        public int SequentialNumber { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CustomerType { get; set; } = null!;
        public AddressDto? MailingAddress { get; set; }
        public AddressDto? ShippingAddress { get; set; }
        public List<SalesOrderDto> Orders { get; set; } = new List<SalesOrderDto>();

        // Residential-specific
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsSeniorDiscountEligible { get; set; }

        // Corporate-specific
        public string? CompanyName { get; set; }
        public int EmployeeCount { get; set; }
        public string? Industry { get; set; }

        // Government-specific
        public string? AgencyName { get; set; }
        public string? Department { get; set; }
        public bool IsFederal { get; set; }
    }
}