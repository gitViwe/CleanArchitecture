using Core.Request.Identity;
using Shared.Wrapper;

namespace Client.Infrastructure.Manager.Account
{
    /// <summary>
    /// A helper to interface with the account API
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Change / update the user password
        /// </summary>
        /// <param name="request">This is the required user information to update password</param>
        /// <returns>A response message</returns>
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest request);

        /// <summary>
        /// Update the user profile details
        /// </summary>
        /// <param name="request">This is the required user information to update profile</param>
        /// <returns>A response message</returns>
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest request);
    }
}