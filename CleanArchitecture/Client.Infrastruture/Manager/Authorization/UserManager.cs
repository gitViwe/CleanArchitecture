using Client.Infrastructure.Extensions;
using Core.Response.Identity;
using Shared.Wrapper;

namespace Client.Infrastructure.Manager.Authorization
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<IEnumerable<UserResponse>>> GetAllAsync()
        {
            // make a get request to the API end point
            var response = await _httpClient.GetAsync(Route.AuthorizationEndpoints.GetAllUsers);

            // process the response into a collection of 'IdentityUser' objects
            return await response.ToResultAsync<IEnumerable<UserResponse>>();
        }

        public async Task<IResult<IEnumerable<string>>> GetRolesAsync(string email)
        {
            // make a get request to the API end point
            var response = await _httpClient.GetAsync(Route.AuthorizationEndpoints.GetUserRoles(email));

            // process the response into a collection of 'IdentityRole' objects
            return await response.ToResultAsync<IEnumerable<string>>();
        }
    }
}
