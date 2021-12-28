using Core.Request.Identity;
using Shared.Wrapper;

namespace Infrastructure.Service
{
    /// <summary>
    /// Facilitates managing the user profile
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Change / update the user password
        /// </summary>
        /// <param name="request">This is the required user information to update password</param>
        /// <param name="userID">This is the user ID from the claims</param>
        /// <returns>A response message</returns>
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest request, string userID);

        /// <summary>
        /// Update the user profile details
        /// </summary>
        /// <param name="request">This is the required user information to update profile</param>
        /// <param name="userID">This is the user ID from the claims</param>
        /// <returns>A response message</returns>
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest request, string userID);
    }
}