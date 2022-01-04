using Client.Infrastructure.Extensions;
using Core.Response;
using Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Client.Infrastructure.Manager.Authorization
{
    public class RoleManager : IRoleManager
    {
        private readonly HttpClient _httpClient;

        public RoleManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<IEnumerable<IdentityRole>>> GetAllAsync()
        {
            // make a get request to the API end point
            var response = await _httpClient.GetAsync(Route.AuthorizationEndpoints.GetAllRoles);

            // process the response into a collection of 'IdentityRole' objects
            return await response.ToResultAsync<IEnumerable<IdentityRole>>();
        }

        public async Task<IResult> CreateAsync(string roleName)
        {
            // make a get request to the API end point
            var response = await _httpClient.PostAsJsonAsync(Route.AuthorizationEndpoints.CreateRole, roleName);

            // process the response into a 'Result' object
            return await response.ToResultAsync();
        }
    }
}
