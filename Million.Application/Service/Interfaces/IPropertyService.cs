using Million.Application.DTOs;

namespace Million.Application.Service.Interfaces
{
    public interface IPropertyService
    {
        Task<PropertyDto> CreateAsync(CreatePropertyRequest request, CancellationToken ct);
        Task<PropertyDto?> UpdateAsync(int id, UpdatePropertyRequest request, CancellationToken ct);
        Task<bool> ChangePriceAsync(int id, ChangePriceRequest request, CancellationToken ct);
        Task<bool> AddImageAsync(int id, AddImageRequest request, CancellationToken ct);
        Task<PagedResult<PropertyDto>> ListAsync(PropertyListRequest request, CancellationToken ct);
        Task<PropertyDto?> GetAsync(int id, CancellationToken ct);
    }
}
