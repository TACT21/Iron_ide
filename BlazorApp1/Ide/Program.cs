using Ide;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using BlazorDownloadFile;
using Microsoft.Extensions.DependencyInjection;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQzNzk4QDMxMzkyZTMzMmUzMEtZb09ZWW9GbEZjTDE2UGVjbDAyYVFSSmNaSHNUcGhGQ1hiK3RFQWpWSUk9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazorDownloadFile();


await builder.Build().RunAsync();
