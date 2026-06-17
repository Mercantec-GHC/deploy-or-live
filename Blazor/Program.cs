using Blazor;
using Blazor.Interfaces;
using Blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
	?? throw new InvalidOperationException("ApiBaseUrl is missing from configuration.");

builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri(apiBaseUrl)
});


builder.Services.AddScoped<INotesApiClient, NotesApiClient>();
builder.Services.AddScoped<IHealthApiClient, HealthApiClient>();
builder.Services.AddScoped<IDocumentationApiClient, DocumentationApiClient>();
await builder.Build().RunAsync();
