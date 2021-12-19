using Core.Request;
using Shared.Wrapper;

namespace Infrastructure.Service
{
    /// <summary>
    /// Facilitates the authentication of system users
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Login an existing user
        /// </summary>
        /// <param name="loginRequest">This is the required user information to login</param>
        /// <returns></returns>
        Task<IResult> LoginUserAsync(LoginRequest loginRequest);

        /// <summary>
        /// Request a new token if the current token is invalid
        /// </summary>
        /// <param name="tokenRequest">This is required user information to verify the user and issue a new token</param>
        /// <returns></returns>
        Task<IResult> RefreshUserTokenAsync(TokenRequest tokenRequest);

        /// <summary>
        /// Register a new user on the system
        /// </summary>
        /// <param name="registrationRequest">This is the required user information to register</param>
        /// <returns></returns>
        Task<IResult> RegisterUserAsync(RegistrationRequest registrationRequest);
    }
}