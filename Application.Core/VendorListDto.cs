namespace Application
{
    public class VendorListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public AddressDto? Address { get; set; }
    }
}