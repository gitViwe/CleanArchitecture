﻿using Client.Infrastructure.Authentication;
using Client.Infrastructure.Manager.Authentication;
using Client.Infrastructure.Manager.Forecast;
using Client.Infrastructure.Manager.Interceptor;
using Client.Infrastructure.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

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
            // register manager services
            services.AddTransient<IWeatherForecastManager, WeatherForecastManager>();
            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddTransient<IHttpInterceptorManager, HttpInterceptorManager>();

            // add a named HTTP client and handler
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("PWA.Client").EnableIntercept(sp))
                .AddHttpClient("PWA.Client", client =>
                {
                    client.BaseAddress = new Uri(configuration["AppConfiguration:ServerUrl"].TrimEnd('/'));
                })
                .AddHttpMessageHandler<ClientAuthenticationHeader>();

            services.AddHttpClientInterceptor();

            return services;
        }
    }
}
