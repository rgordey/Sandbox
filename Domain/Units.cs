namespace Domain
{
    public enum WeightUnit
    {
        Kg,
        Lbs
    }

    public enum DimensionUnit
    {
        Cm,
        Inches
    }

    public static class UnitConverters
    {
        // Convert weight to a base unit (e.g., kg)
        public static decimal ToKg(decimal value, WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Lbs => value * 0.453592m,
                _ => value
            };
        }

        // Convert from kg back to target unit
        public static decimal FromKg(decimal valueInKg, WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Lbs => valueInKg / 0.453592m,
                _ => valueInKg
            };
        }

        // Similar for dimensions, base unit: cm
        public static decimal ToCm(decimal value, DimensionUnit unit)
        {
            return unit switch
            {
                DimensionUnit.Inches => value * 2.54m,
                _ => value
            };
        }

        public static decimal FromCm(decimal valueInCm, DimensionUnit unit)
        {
            return unit switch
            {
                DimensionUnit.Inches => valueInCm / 2.54m,
                _ => valueInCm
            };
        }
    }
}