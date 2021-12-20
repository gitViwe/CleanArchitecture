using Client;
using Client.Infrastructure.Manager.Demo;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// add MudBlazor service https://mudblazor.com/getting-started/installation
builder.Services.AddMudServices();

// add API 'Weather Forecast' service
builder.Services.AddHttpClient<IWeatherForecastManager, WeatherForecastManager>
    // and set the URL of the API client
    (client => client.BaseAddress = new Uri("https://localhost:7100"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
