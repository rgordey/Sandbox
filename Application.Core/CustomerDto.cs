﻿namespace Application
{
    // DTOs
    public sealed class CustomerDto
    {
        public Guid Id { get; set; }
        public string? CustomerNumber { get; set; }
        public int SequentialNumber { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CustomerType { get; set; } = null!;        
        public AddressDto? MailingAddress { get; set; }
        public AddressDto? ShippingAddress { get; set; }
        public List<SalesOrderDto> Orders { get; set; } = new List<SalesOrderDto>();        
    }

    public sealed class CustomerListDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CustomerType { get; set; } = null!;        
    }
}