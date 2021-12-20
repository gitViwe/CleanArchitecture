using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Extensions
{
    /// <summary>
    /// Implementation of the services registered in the <see cref="Program"/> class
    /// </summary>
    public static class WebAssemblyHostBuilderExtensions
    {
        public static void RegisterClientServices(this IServiceCollection services)
        {
            // add API 'Weather Forecast' service
        }
    }
}
