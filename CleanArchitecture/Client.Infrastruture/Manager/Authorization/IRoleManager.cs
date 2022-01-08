using Core.Response.Identity;
using Shared.Wrapper;

namespace Client.Infrastructure.Manager.Authorization
{
    /// <summary>
    /// A helper to interface with the Authorization API
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Create a new role on the system
        /// </summary>
        /// <param name="roleName">This is the name of the Identity Role to create</param>
        /// <returns>A response message</returns>
        Task<IResult> CreateAsync(string roleName);

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>A list of the Identity roles</returns>
        Task<IResult<IEnumerable<RoleResponse>>> GetAllAsync();
    }
}