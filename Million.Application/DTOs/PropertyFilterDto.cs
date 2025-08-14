using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.DTOs
{
    /// <summary>
    /// DTO para filtrar propiedades en listados/paginación
    /// </summary>
    public record PropertyFilterDto
    {
        public int? OwnerId { get; init; }
        public decimal? PriceMin { get; init; }
        public decimal? PriceMax { get; init; }
        public short? Year { get; init; }
        public string? Code { get; init; }
        public string? Name { get; init; }
        public bool? IsActive { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
