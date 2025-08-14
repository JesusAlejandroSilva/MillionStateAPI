namespace Million.Domain.Entities
{
    /// <summary>Inmueble (tabla dbo.Property)</summary>
    public class Property
    {
        public int IdProperty { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = default!;
        public short? Year { get; set; }
        public int IdOwner { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Owner? Owner { get; set; }
        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public ICollection<PropertyTrace> Traces { get; set; } = new List<PropertyTrace>();

        /// <summary>Regla de dominio para cambiar el precio (valida valor positivo)</summary>
        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0) throw new ArgumentOutOfRangeException(nameof(newPrice), "Price must be > 0");
            Price = newPrice;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}