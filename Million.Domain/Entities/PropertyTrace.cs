namespace Million.Domain.Entities
{
    /// <summary>Bitácora/venta/ajuste (tabla dbo.PropertyTrace)</summary>
    public class PropertyTrace
    {
        public int IdPropertyTrace { get; set; }
        public DateOnly DateSale { get; set; }
        public string Name { get; set; } = default!;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public int IdProperty { get; set; }
        public DateTime CreatedAt { get; set; }

        public Property? Property { get; set; }
    }
}
