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

        /// <summary>
        /// Clears all credentials on the client
        /// </summary>
        /// <returns>A success flag</returns>
        Task<IResult> Logout();

        /// <summary>
        /// Attempts to get a new JWT token
        /// </summary>
        /// <returns>A response message</returns>
        Task<IResult> RefreshToken();

        /// <summary>
        /// Send a registration request to the API
        /// </summary>
        /// <param name="request">The user details required for registration</param>
        /// <returns>The response message</returns>
        Task<IResult> Register(RegistrationRequest request);
    }
}