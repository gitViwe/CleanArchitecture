using Client.Infrastructure.Authentication;
using Client.Infrastructure.Manager.Authentication;
using Client.Infrastructure.Manager.Demo;
using Client.Infrastructure.Service;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Extensions
{
    /// <summary>
    /// Implementation of the services registered in the <see cref="Program"/> class
    /// </summary>
    public static class WebAssemblyHostBuilderExtensions
    {
        /// <summary>
        /// Registers the services required by the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ClientAuthenticationHeader>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();

            return services;
        }

        /// <summary>
        /// Registers a custom authentication state provider
        /// </summary>
        public static IServiceCollection RegisterAuthenticationProvider(this IServiceCollection services)
        {
            // register 'ClientStateProvider' in DI container
            services.AddScoped<ClientStateProvider>();
            // use 'ClientStateProvider' when system requests 'AuthenticationStateProvider'
            services.AddScoped<AuthenticationStateProvider, ClientStateProvider>();

            return services;
        }

        /// <summary>
        /// Registers the HTTP client managers for the application
        /// </summary>
        public static IServiceCollection RegisterHttpClientManagers(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient<IWeatherForecastManager, WeatherForecastManager>
                (client => client.BaseAddress = new Uri(configuration["AppConfiguration:ServerUrl"].TrimEnd('/')))
                .AddHttpMessageHandler<ClientAuthenticationHeader>();

            services.AddHttpClient<IAuthenticationManager, AuthenticationManager>
                (client => client.BaseAddress = new Uri(configuration["AppConfiguration:ServerUrl"].TrimEnd('/')))
                .AddHttpMessageHandler<ClientAuthenticationHeader>();

            return services;
        }
    }
}
