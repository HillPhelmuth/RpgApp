using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RpgApp.Shared;
using RpgApp.Shared.Services;

namespace RpgApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
            });
            builder.Services.AddModalDialog();
            builder.Services.AddScoped<CombatService>();
            builder.Services.AddSingleton<AppStateManager>();
            await builder.Build().RunAsync();
        }
    }
}
