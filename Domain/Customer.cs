﻿namespace Domain
{
    // Entity Models
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}


