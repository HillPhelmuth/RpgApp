using Microsoft.Extensions.DependencyInjection;

namespace RpgComponentLibrary.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddRpgComponentInterop(this IServiceCollection service)
        {
            service.AddScoped<GuiInterop>();
            return service;
        }
    }
}
