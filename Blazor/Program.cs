using Blazor;
using Blazor.Interfaces;
using Blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// In Development, ApiBaseUrl points directly at the API (e.g. http://localhost:5012/).
// In Production it is left empty so requests are same-origin and nginx proxies /api
// to the API container, which avoids CORS and hardcoded domains.
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
	apiBaseUrl = builder.HostEnvironment.BaseAddress;
}

builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri(apiBaseUrl)
});

builder.Services.AddScoped<IHealthApiClient, HealthApiClient>();
builder.Services.AddScoped<IDocumentationApiClient, DocumentationApiClient>();
await builder.Build().RunAsync();
