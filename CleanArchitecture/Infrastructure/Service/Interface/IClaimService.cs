using Core.Request.Identity;
using Shared.Wrapper;

namespace Infrastructure.Service
{
    /// <summary>
    /// Facilitates managing the user and role claims
    /// </summary>
    public interface IClaimService
    {
        /// <summary>
        /// Add a claim to a specific role
        /// </summary>
        /// <param name="request">This is the required role and claims information to process the request</param>
        /// <returns>A response message</returns>
        Task<IResult> AddClaimToRoleAsync(RoleClaimRequest request);

        /// <summary>
        /// Add a claim to a specific user
        /// </summary>
        /// <param name="request">This is the required user and claims information to process the request</param>
        /// <returns>A response message</returns>
        Task<IResult> AddClaimToUserAsync(UserClaimRequest request);

        /// <summary>
        /// Get all claims associated with the specific role
        /// </summary>
        /// <param name="roleName">This is the name of the Identity Role</param>
        /// <returns>A list of claims</returns>
        Task<IResult> GetRoleClaimsAsync(string roleName);

        /// <summary>
        /// Get all claims associated with the specific user
        /// </summary>
        /// <param name="email">This is the email address belonging to the user</param>
        /// <returns>A list of claims</returns>
        Task<IResult> GetUserClaimsAsync(string email);

        /// <summary>
        /// Remove a claim from the role
        /// </summary>
        /// <param name="request">This is the required role and claims information to process the request</param>
        /// <returns>A response message</returns>
        Task<IResult> RemoveRoleClaimAsync(RoleClaimRequest request);

        /// <summary>
        /// Remove a claim from the user
        /// </summary>
        /// <param name="request">This is the required user and claims information to process the request</param>
        /// <returns>A response message</returns>
        Task<IResult> RemoveUserClaimAsync(UserClaimRequest request);
    }
}