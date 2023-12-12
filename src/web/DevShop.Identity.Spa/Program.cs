using CurrieTechnologies.Razor.SweetAlert2;
using DevShop.Identity.Spa;
using DevShop.Identity.Spa.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<MenuService>();

builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
