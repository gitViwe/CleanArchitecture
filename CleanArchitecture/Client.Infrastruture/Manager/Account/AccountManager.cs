using Client.Infrastructure.Extensions;
using Client.Infrastructure.Manager.Authentication;
using Core.Request.Identity;
using Shared.Wrapper;
using System.Net.Http.Json;

namespace Client.Infrastructure.Manager.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountManager(
            HttpClient httpClient,
            IAuthenticationManager authenticationManager)
        {
            _httpClient = httpClient;
            _authenticationManager = authenticationManager;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            // make a get request to the API end point
            var response = await _httpClient.PutAsJsonAsync(Route.AccountEndpoints.ChangePassword, request);

            // process the response into a 'Result' object
            var result = await response.ToResultAsync();

            if (result.Succeeded)
            {
                var refreshed = await _authenticationManager.TryRefreshTokenAsync();

                if (refreshed == false)
                {
                    await _authenticationManager.LogoutAsync();
                }
            }

            return result;
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest request)
        {
            // make a get request to the API end point
            var response = await _httpClient.PutAsJsonAsync(Route.AccountEndpoints.UpdateProfile, request);

            // process the response into a 'Result' object
            var result = await response.ToResultAsync();

            if (result.Succeeded)
            {
                var refreshed = await _authenticationManager.TryRefreshTokenAsync();

                if (refreshed == false)
                {
                    // TODO: force token refresh ?
                }
            }

            return result;
        }
    }
}
