using Microsoft.Extensions.DependencyInjection;
using Million.Application.Service;
using Million.Application.Service.Interfaces;
using System.Reflection;

namespace Million.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper solo escanea esta capa
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Registrar tus servicios de Application
            services.AddScoped<IPropertyService, PropertyService>();

            return services;
        }
    }
}
