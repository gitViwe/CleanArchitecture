using Client.Infrastruture.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Constant.Permission;
using Shared.Constant.Storage;
using Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Infrastruture.Authentication
{
    /// <summary>
    /// This class provides information abou the athentication state of the current user. Inherits from <see cref="AuthenticationStateProvider"/>
    /// </summary>
    public class ClientStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpService _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ClientStateProvider(
            IHttpService httpClient,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // get the saved JWT token
            var savedToken = await _localStorage.GetItemAsync<string>(ClientStorage.Local.AuthToken);

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                // return empty credentials if no token found
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // get the authentication state using the saved token
            var authSatate = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));


            throw new NotImplementedException();
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
