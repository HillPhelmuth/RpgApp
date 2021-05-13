using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
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
            // WHen we secure the back-end of the app, we will create an HttpClient that will attach the Auth token to the request automatically. We'll have to create an AuthClient class that contains a read-only HttpClient that we then use to access the controller.
            //builder.Services.AddHttpClient<AuthClient>(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
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
