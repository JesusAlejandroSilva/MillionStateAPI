namespace Million.Application.DTOs
{
    public record CreatePropertyRequest(
        string Name, string Address, decimal Price, string CodeInternal, short? Year, int IdOwner);

    public record UpdatePropertyRequest(
        string Name, string Address, short? Year, bool IsActive, int IdOwner);

    public record ChangePriceRequest(decimal NewPrice);
    public record AddImageRequest(string File, bool Enabled = true);

    public record PropertyImageDto(int IdPropertyImage, string File, bool Enabled, DateTime CreatedAt);

    public record PropertyDto(
        int IdProperty, string Name, string Address, decimal Price, string CodeInternal, short? Year,
        int IdOwner, bool IsActive, DateTime CreatedAt, DateTime? UpdatedAt, IEnumerable<PropertyImageDto> Images);

    public record PropertyListRequest(
        int? OwnerId, decimal? PriceMin, decimal? PriceMax, short? Year, string? Code, string? Name,
        bool? IsActive, int Page = 1, int PageSize = 20);

    public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int Total);
}
