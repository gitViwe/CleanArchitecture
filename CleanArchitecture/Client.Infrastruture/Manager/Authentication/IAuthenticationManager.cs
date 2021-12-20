using Core.Request;
using Shared.Wrapper;
using System.Security.Claims;

namespace Client.Infrastructure.Manager.Authentication
{
    /// <summary>
    /// A helper to interface with the authentication API
    /// </summary>
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Get the current user
        /// </summary>
        /// <returns>The claims principal representing the current user</returns>
        Task<ClaimsPrincipal> CurrentUser();

        /// <summary>
        /// Send a login request to the API
        /// </summary>
        /// <param name="request">The user details required for login</param>
        /// <returns>The response message</returns>
        Task<IResult> Login(LoginRequest request);
    }
}