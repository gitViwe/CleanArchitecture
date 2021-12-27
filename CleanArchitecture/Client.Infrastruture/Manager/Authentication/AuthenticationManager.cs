using Client.Infrastructure.Authentication;
using Client.Infrastructure.Extensions;
using Client.Infrastructure.Service;
using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Constant.Storage;
using Shared.Wrapper;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Client.Infrastructure.Manager.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ClaimsPrincipal> CurrentUserAsync()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResult> LoginAsync(LoginRequest request)
        {
            // make a get request to the API end point
            var response = await _httpClient.PostAsJsonAsync(Route.AuthenticationEndpoints.Login, request);

            // process the response into a 'Result' object
            var result = await response.ToResultAsync<AuthenticationResponse>();

            if (result.Succeeded)
            {
                // get tokens from response data
                var token = result.Data.Token;
                var refreshToken = result.Data.RefreshToken;

                // store tokens on the client side
                await _localStorage.SetItemAsync(ClientStorage.Local.AuthToken, token);
                await _localStorage.SetItemAsync(ClientStorage.Local.RefreshToken, refreshToken);

                // update the authentication state
                await ((ClientStateProvider)_authenticationStateProvider).StateChangedAsync();

                return Result.Success();
            }

            return Result.Fail(result.Messages);
        }

        public async Task<IResult> RegisterAsync(RegistrationRequest request)
        {
            // make a get request to the API end point
            var response = await _httpClient.PostAsJsonAsync(Route.AuthenticationEndpoints.Register, request);

            // process the response into a 'Result' object
            var result = await response.ToResultAsync<AuthenticationResponse>();

            if (result.Succeeded)
            {
                // get tokens from response data
                var token = result.Data.Token;
                var refreshToken = result.Data.RefreshToken;

                // store tokens on the client side
                await _localStorage.SetItemAsync(ClientStorage.Local.AuthToken, token);
                await _localStorage.SetItemAsync(ClientStorage.Local.RefreshToken, refreshToken);

                // update the authentication state
                await ((ClientStateProvider)_authenticationStateProvider).StateChangedAsync();

                return Result.Success();
            }

            return Result.Fail(result.Messages);
        }

        public async Task LogoutAsync()
        {
            // remove stored tokens
            await _localStorage.RemoveItemAsync(ClientStorage.Local.AuthToken);
            await _localStorage.RemoveItemAsync(ClientStorage.Local.RefreshToken);

            // update the authentication state
            ((ClientStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }

        public async Task<IResult> RefreshTokenAsync()
        {
            // get tokens from client storage
            var token = await _localStorage.GetItemAsync<string>(ClientStorage.Local.AuthToken);
            var refreshToken = await _localStorage.GetItemAsync<string>(ClientStorage.Local.RefreshToken);

            // create model for API request
            var request = new TokenRequest()
            {
                Token = token,
                RefreshToken = refreshToken
            };

            // make a get request to the API end point
            var response = await _httpClient.PostAsJsonAsync(Route.AuthenticationEndpoints.RefreshToken, request);

            // process the response into a 'Result' object
            var result = await response.ToResultAsync<AuthenticationResponse>();

            if (result.Succeeded)
            {
                // get tokens from response data
                token = result.Data.Token;
                refreshToken = result.Data.RefreshToken;

                // store tokens on the client side
                await _localStorage.SetItemAsync(ClientStorage.Local.AuthToken, token);
                await _localStorage.SetItemAsync(ClientStorage.Local.RefreshToken, refreshToken);

                // update the authentication state
                await ((ClientStateProvider)_authenticationStateProvider).StateChangedAsync();

                return Result.Success(token);
            }

            return Result.Fail(result.Messages);
        }
    }
}
