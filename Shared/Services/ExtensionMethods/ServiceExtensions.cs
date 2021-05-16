using Microsoft.Extensions.DependencyInjection;

namespace RpgApp.Shared.Services.ExtensionMethods
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRpgServices(this IServiceCollection services)
        {
            services.AddScoped<CombatService>();
            services.AddScoped<RpgLocalStorageService>();
            services.AddSingleton<AppStateManager>();
            return services;
        }
    }
}