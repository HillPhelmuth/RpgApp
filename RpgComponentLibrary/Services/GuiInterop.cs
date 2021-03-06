using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RpgComponentLibrary.Services
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class GuiInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public GuiInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/RpgComponentLibrary/rpgui-helper.js").AsTask());
        }

       
        public async ValueTask<double> GetWidth(ElementReference element)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<double>("getWidth", element);
        }
        //public async ValueTask UpdateProgressBar(ElementReference element, double value)
        //{
        //    var module = await moduleTask.Value;
        //    await module.InvokeAsync<string>("setValue", element, value);
        //}
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
