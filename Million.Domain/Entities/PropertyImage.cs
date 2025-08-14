namespace Million.Domain.Entities
{
    /// <summary>Imagen de inmueble (tabla dbo.PropertyImage)</summary>
    public class PropertyImage
    {
        public int IdPropertyImage { get; set; }
        public int IdProperty { get; set; }
        public string File { get; set; } = default!;
        public bool Enabled { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public Property? Property { get; set; }
    }
}
