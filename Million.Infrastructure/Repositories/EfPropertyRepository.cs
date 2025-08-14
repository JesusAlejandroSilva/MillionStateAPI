using Microsoft.EntityFrameworkCore;
using Million.Domain.Entities;
using Million.Domain.Ports.Interfaces;
using Million.Infrastructure.Persistence;

namespace Million.Infrastructure.Repositories
{
    public class EfPropertyRepository: IPropertyRepository
    {
        private readonly MillionStateDbContext _db;

        public EfPropertyRepository(MillionStateDbContext db) => _db = db;

        /// <inheritdoc />
        public async Task<Property> AddAsync(Property entity, CancellationToken ct)
        {
            _db.Properties.Add(entity);
            await _db.SaveChangesAsync(ct);

            // Cargar relaciones necesarias para devolver entidad completa
            await _db.Entry(entity).Collection(p => p.Images).LoadAsync(ct);

            return entity;
        }

        /// <inheritdoc />
        public async Task<Property?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _db.Properties
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.IdProperty == id, ct);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Property entity, CancellationToken ct)
        {
            _db.Properties.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        /// <inheritdoc />
        public async Task AddImageAsync(PropertyImage image, CancellationToken ct)
        {
            _db.PropertyImages.Add(image);
            await _db.SaveChangesAsync(ct);
        }

        /// <inheritdoc />
        public async Task<(IReadOnlyList<Property> Items, int Total)> ListAsync(PropertyFilter filter, CancellationToken ct)
        {
            var q = _db.Properties.AsNoTracking()
                                  .Include(p => p.Images)
                                  .AsQueryable();

            // Aplicar filtros
            if (filter.OwnerId.HasValue)
                q = q.Where(p => p.IdOwner == filter.OwnerId.Value);

            if (filter.PriceMin.HasValue)
                q = q.Where(p => p.Price >= filter.PriceMin.Value);

            if (filter.PriceMax.HasValue)
                q = q.Where(p => p.Price <= filter.PriceMax.Value);

            if (filter.Year.HasValue)
                q = q.Where(p => p.Year == filter.Year.Value);

            if (!string.IsNullOrWhiteSpace(filter.Code))
                q = q.Where(p => p.CodeInternal.Contains(filter.Code));

            if (!string.IsNullOrWhiteSpace(filter.Name))
                q = q.Where(p => p.Name.Contains(filter.Name));

            if (filter.IsActive.HasValue)
                q = q.Where(p => p.IsActive == filter.IsActive.Value);

            // Paginación
            var total = await q.CountAsync(ct);

            var items = await q.OrderBy(p => p.IdProperty)
                               .Skip((filter.Page - 1) * filter.PageSize)
                               .Take(filter.PageSize)
                               .ToListAsync(ct);

            return (items, total);
        }
    }
}
