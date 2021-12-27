using Core.Request.Identity;
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
        Task<ClaimsPrincipal> CurrentUserAsync();

        /// <summary>
        /// Send a login request to the API
        /// </summary>
        /// <param name="request">The user details required for login</param>
        /// <returns>The response message</returns>
        Task<IResult> LoginAsync(LoginRequest request);

        /// <summary>
        /// Clears all credentials on the client
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Attempts to get a new JWT token
        /// </summary>
        /// <returns>A token on success or a response message on failure</returns>
        Task<IResult> RefreshTokenAsync();

        /// <summary>
        /// Send a registration request to the API
        /// </summary>
        /// <param name="request">The user details required for registration</param>
        /// <returns>The response message</returns>
        Task<IResult> RegisterAsync(RegistrationRequest request);
    }
}