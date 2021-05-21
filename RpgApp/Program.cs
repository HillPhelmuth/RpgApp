using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RpgApp.Shared.Services;
using RpgApp.Shared.Services.ExtensionMethods;

namespace RpgApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient<ClientDataService>(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
                options.UserOptions.NameClaim = "nickname";
            });
            builder.Services.AddModalDialog();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddRpgServices();
            builder.Services.AddSingleton<RpgCosmosService>();
            await builder.Build().RunAsync();
        }
    }
    
}
