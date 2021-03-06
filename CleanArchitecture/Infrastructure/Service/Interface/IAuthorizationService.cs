using Core.Request.Identity;
using Shared.Wrapper;

namespace Infrastructure.Service
{
    /// <summary>
    /// Facilitates the authorization of system users
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Assign a role to a specific user
        /// </summary>
        /// <param name="request">This is the required user email and role name</param>
        /// <returns>A result message</returns>
        Task<IResult> AddUserToRoleAsync(RoleUserRequest request);

        /// <summary>
        /// Create a new role on the system
        /// </summary>
        /// <param name="request">This is the name and description of the Identity Role to create</param>
        /// <returns>A result message</returns>
        Task<IResult> CreateRoleAsync(RoleRequest request);

        /// <summary>
        /// Modify an existing role on the system
        /// </summary>
        /// <param name="request">This is the name and description of the Identity Role to create</param>
        /// <returns>A result message</returns>
        Task<IResult> UpdateRoleAsync(RoleRequest request);

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>A list of the Identity roles</returns>
        Task<IResult> GetRolesAsync();

        /// <summary>
        /// Get the roles for this user
        /// </summary>
        /// <param name="email">This is the email address belonging to the user</param>
        /// <returns>A list of the Identity roles for this user</returns>
        Task<IResult> GetUserRolesAsync(string email);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>A list of the Identity users</returns>
        Task<IResult> GetUsersAsync();

        /// <summary>
        /// Remove user from this role
        /// </summary>
        /// <param name="request">This is required user email and role name</param>
        /// <returns>A result message</returns>
        Task<IResult> RemoveUserFromRoleAsync(RoleUserRequest request);
    }
}