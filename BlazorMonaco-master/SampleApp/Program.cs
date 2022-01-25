using Blazored.LocalStorage;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorWorker.BackgroundServiceFactory;
using BlazorWorker.Core;

namespace SampleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQzNzk4QDMxMzkyZTMzMmUzMEtZb09ZWW9GbEZjTDE2UGVjbDAyYVFSSmNaSHNUcGhGQ1hiK3RFQWpWSUk9");
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddWorkerFactory();
            await builder.Build().RunAsync();
        }
    }
}
