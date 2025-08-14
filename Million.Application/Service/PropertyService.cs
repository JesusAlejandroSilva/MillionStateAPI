using AutoMapper;
using Million.Application.DTOs;
using Million.Application.Service.Interfaces;
using Million.Domain.Entities;
using Million.Domain.Ports.Interfaces;

namespace Million.Application.Service
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _repo;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PropertyDto> CreateAsync(CreatePropertyRequest request, CancellationToken ct)
        {
            var entity = _mapper.Map<Property>(request);
            entity.CreatedAt = DateTime.UtcNow;
            var added = await _repo.AddAsync(entity, ct);
            return _mapper.Map<PropertyDto>(added);
        }

        public async Task<PropertyDto?> UpdateAsync(int id, UpdatePropertyRequest request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null) return null;
            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(entity, ct);
            return _mapper.Map<PropertyDto>(entity);
        }

        public async Task<bool> ChangePriceAsync(int id, ChangePriceRequest request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null) return false;
            entity.ChangePrice(request.NewPrice);
            await _repo.UpdateAsync(entity, ct);
            return true;
        }

        public async Task<bool> AddImageAsync(int id, AddImageRequest request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null) return false;

            var img = new PropertyImage
            {
                IdProperty = id,
                File = request.File,
                Enabled = request.Enabled,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddImageAsync(img, ct);
            return true;
        }

        public async Task<PagedResult<PropertyDto>> ListAsync(PropertyListRequest request, CancellationToken ct)
        {
            var filter = new PropertyFilter
            {
                OwnerId = request.OwnerId,
                PriceMin = request.PriceMin,
                PriceMax = request.PriceMax,
                Year = request.Year,
                Code = request.Code,
                Name = request.Name,
                IsActive = request.IsActive,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var (items, total) = await _repo.ListAsync(filter, ct);
            var dtos = items.Select(p => _mapper.Map<PropertyDto>(p)).ToList();
            return new PagedResult<PropertyDto>(dtos, request.Page, request.PageSize, total);
        }

        public async Task<PropertyDto?> GetAsync(int id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<PropertyDto>(entity);
        }
    }
}