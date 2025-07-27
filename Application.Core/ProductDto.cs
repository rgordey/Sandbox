using Domain;

namespace Application
{
    public sealed class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal BasePrice { get; set; } // Optional base price, if applicable
        public decimal RetailPrice { get; set; }  // Calculated retail price (with standard markup on min cost)
        public decimal Weight { get; set; }
        public WeightUnit WeightUnit { get; set; } = WeightUnit.Kg;
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public DimensionUnit DimensionUnit { get; set; } = DimensionUnit.Cm;
        public List<VendorDto> Vendors { get; set; } = new(); // List of vendors supplying this product
    }
}