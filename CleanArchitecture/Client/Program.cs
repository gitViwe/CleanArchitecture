using Client;
using Client.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// authorization services
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

// add MudBlazor service https://mudblazor.com/getting-started/installation
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
});

// register services using extension methods
builder.Services.RegisterApplicationServices();
builder.Services.RegisterAuthenticationProvider();
builder.Services.RegisterHttpClientManagers(builder.Configuration);

await builder.Build().RunAsync();
