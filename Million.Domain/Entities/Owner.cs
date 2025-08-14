namespace Million.Domain.Entities
{
    /// <summary>Propietario (tabla dbo.Owner)</summary>
    public class Owner
    {
        public int IdOwner { get; set; }
        public string Name { get; set; } = default!;
        public string? Address { get; set; }
        public string? Photo { get; set; }
        public DateOnly? Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
