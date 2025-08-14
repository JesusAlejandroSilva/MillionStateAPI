using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Million.Application.Ports.Interfaces;
using Million.Domain.Ports.Interfaces;
using Million.Infrastructure.Auth;
using Million.Infrastructure.Common;
using Million.Infrastructure.Persistence;
using Million.Infrastructure.Repositories;

namespace Million.Infrastructure.Configuration
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // DbContext
            services.AddDbContext<MillionStateDbContext>(opt =>
                opt.UseSqlServer(connectionString));

            // Repositorios
            services.AddScoped<IPropertyRepository, EfPropertyRepository>();

            // Singletons
            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}
