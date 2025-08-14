using Million.Domain.Entities;

namespace Million.Domain.Ports.Interfaces
{
    public interface IPropertyRepository
    {
       Task<Property> AddAsync(Property entity, CancellationToken ct);
       Task<Property?> GetByIdAsync(int id, CancellationToken ct);
       Task UpdateAsync(Property entity, CancellationToken ct);
       Task AddImageAsync(PropertyImage image, CancellationToken ct);
       Task<(IReadOnlyList<Property> Items, int Total)> ListAsync(PropertyFilter filter, CancellationToken ct);
        
    }
    public sealed class PropertyFilter
    {
        public int? OwnerId { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public short? Year { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
