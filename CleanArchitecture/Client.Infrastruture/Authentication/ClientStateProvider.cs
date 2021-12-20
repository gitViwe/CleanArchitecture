using Client.Infrastruture.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Constant.Permission;
using Shared.Constant.Storage;
using Shared.Utility;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Client.Infrastruture.Authentication
{
    /// <summary>
    /// This class provides information abou the athentication state of the current user. Inherits from <see cref="AuthenticationStateProvider"/>
    /// </summary>
    public class ClientStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ClientStateProvider(
            HttpClient httpClient,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        /// <summary>
        /// The current user's claims principal
        /// </summary>
        public ClaimsPrincipal AuthenticationStateUser { get; private set; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // get the saved JWT token
            var savedToken = await _localStorage.GetItemAsync<string>(ClientStorage.Local.AuthToken);

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                // return empty credentials if no token found
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // use the saved token as the default authorization header value
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            // get the authentication state using the saved token
            var authSatate = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));

            // get the authentication state user value
            AuthenticationStateUser = authSatate.User;

            return authSatate;
        }

        /// <summary>
        /// Gets the current authentication state user
        /// </summary>
        /// <returns>The current user's claims principal</returns>
        public async Task<ClaimsPrincipal> GetAuthenticationStateUserAsync()
        {
            var authState = await this.GetAuthenticationStateAsync();

            var authStateUser = authState.User;

            return authStateUser;
        }

        /// <summary>
        /// Change the authentication state when user logs out
        /// </summary>
        public void MarkUserAsLoggedOut()
        {
            // create blank user claims principal
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            // create authentication state using blank user
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            // update the authentication state
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// Change the authentication state
        /// </summary>
        public async Task StateChangedAsync()
        {
            // verify the current authentication state
            var authState = Task.FromResult(await GetAuthenticationStateAsync());

            // update the authentication state
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            // instantiates the list of claims to return
            var output = new List<Claim>();

            // seperate the token string
            var payload = jwt.Split('.')[1];

            // get the byte array from the token string
            var jsonBytes = Conversion.ParseBase64WithoutPadding(payload);

            // get the key value pairs for claims from the byte array
            var claimsDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);


            if (claimsDictionary is not null)
            {
                // get all the role claim types from the dictionary
                claimsDictionary.TryGetValue(ClaimTypes.Role, out var roles);

                if (roles is not null)
                {
                    // if roles start with '[' then this is an array
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        // get roles as string array
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                        // add the array to the claims list
                        output.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        // add the role to the claims list
                        output.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }

                    // removed roles from dictionary to prevent adding diplicates
                    claimsDictionary.Remove(ClaimTypes.Role);
                }

                // get all the permission claim types from the dictionary
                claimsDictionary.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);

                if (permissions is not null)
                {
                    // if permissions start with '[' then this is an array
                    if (permissions.ToString().Trim().StartsWith("["))
                    {
                        // get roles as string array
                        var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());

                        // add the array to the claims list
                        output.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
                    }
                    else
                    {
                        // add the permission to the claims list
                        output.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
                    }

                    // removed permissions from dictionary to prevent adding diplicates
                    claimsDictionary.Remove(ApplicationClaimTypes.Permission);
                }

                // add all the remaining claims to the claims list
                output.AddRange(claimsDictionary.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }

            return output;
        }

    }
}
