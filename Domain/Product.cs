using System.Reflection.Metadata;

namespace Domain
{
    public sealed class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal BasePrice { get; set; }

        public decimal Weight { get; set; }
        public WeightUnit WeightUnit { get; set; } = WeightUnit.Kg;
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public DimensionUnit DimensionUnit { get; set; } = DimensionUnit.Cm;

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<ProductVendor> ProductVendors { get; set; } = new();
    }
}


