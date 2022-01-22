using Client.Infrastructure.Extensions;
using Core.Request.Identity;
using Core.Response.Identity;
using Shared.Wrapper;
using System.Net.Http.Json;

namespace Client.Infrastructure.Manager.Authorization
{
    public class RoleManager : IRoleManager
    {
        private readonly HttpClient _httpClient;

        public RoleManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<IEnumerable<RoleResponse>>> GetAllAsync()
        {
            // make a get request to the API end point
            var response = await _httpClient.GetAsync(Route.AuthorizationEndpoints.GetAllRoles);

            // process the response into a collection of 'IdentityRole' objects
            return await response.ToResultAsync<IEnumerable<RoleResponse>>();
        }

        public async Task<IResult> CreateAsync(RoleRequest request)
        {
            // make a get request to the API end point
            var response = await _httpClient.PostAsJsonAsync(Route.AuthorizationEndpoints.CreateRole, request);

            // process the response into a 'Result' object
            return await response.ToResultAsync();
        }

        public async Task<IResult> UpdateAsync(RoleRequest request)
        {
            // make a get request to the API end point
            var response = await _httpClient.PostAsJsonAsync(Route.AuthorizationEndpoints.UpdateRole, request);

            // process the response into a 'Result' object
            return await response.ToResultAsync();
        }
    }
}
