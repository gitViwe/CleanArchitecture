using Client.Infrastructure.Manager.Authentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace Client.Infrastructure.Manager.Interceptor
{
    // https://code-maze.com/refresh-token-with-blazor-webassembly-and-asp-net-core-web-api/
    public class HttpInterceptorManager : IHttpInterceptorManager
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackBar;

        public HttpInterceptorManager(
            HttpClientInterceptor interceptor,
            IAuthenticationManager authenticationManager,
            NavigationManager navigationManager,
            ISnackbar snackBar)
        {
            _interceptor = interceptor;
            _authenticationManager = authenticationManager;
            _navigationManager = navigationManager;
            _snackBar = snackBar;
        }

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            // check the request URI of the intercepted request
            var absPath = e.Request.RequestUri.AbsolutePath;

            // request should not be the one we use for the refresh token or login/logout action
            if (absPath.Contains("Authentication", StringComparison.InvariantCultureIgnoreCase) == false)
            {
                try
                {
                    // get the refresh token
                    var result = await _authenticationManager.RefreshTokenAsync();

                    if (result.Succeeded)
                    {
                        _snackBar.Add("Token refreshed.", Severity.Success);
                        // use the new token as the authorization header value
                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.Messages.FirstOrDefault());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _snackBar.Add("You are Logged Out.", Severity.Error);
                    await _authenticationManager.LogoutAsync();
                    _navigationManager.NavigateTo("/");
                }
            }
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}
