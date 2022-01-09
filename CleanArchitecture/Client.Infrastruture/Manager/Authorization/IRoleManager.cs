using Core.Request.Identity;
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
        /// <param name="request">This is the name and description of the Identity Role to create</param>
        /// <returns>A response message</returns>
        Task<IResult> CreateAsync(RoleRequest request);

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>A list of the Identity roles</returns>
        Task<IResult<IEnumerable<RoleResponse>>> GetAllAsync();
    }
}