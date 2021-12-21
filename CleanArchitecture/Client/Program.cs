using Client;
using Client.Extensions;
using Client.Infrastructure.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// authorization services
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

// add MudBlazor service https://mudblazor.com/getting-started/installation
builder.Services.AddMudServices();

// register services using extension methods
builder.Services.RegisterApplicationServices();
builder.Services.RegisterAuthenticationProvider();
builder.Services.RegisterHttpClientManagers(builder.Configuration);

await builder.Build().RunAsync();
