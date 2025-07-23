namespace Domain
{
    public class Address
    {
        public string Line1 { get; set; } = null!;
        public string? Line2 { get; set; } // Optional field for additional address information
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string Country { get; set; } = "US"; // Added country field for international addresses
    }
}
