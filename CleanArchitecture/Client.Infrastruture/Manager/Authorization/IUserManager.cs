using Core.Response.Identity;
using Shared.Wrapper;

namespace Client.Infrastructure.Manager.Authorization
{
    /// <summary>
    /// A helper to interface with the Authorization API
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>A list of the Identity users</returns>
        Task<IResult<IEnumerable<UserResponse>>> GetAllAsync();

        /// <summary>
        /// Get all roles assigned to the user
        /// </summary>
        /// <param name="email">Te email address of the user</param>
        /// <returns></returns>
        Task<IResult<IEnumerable<string>>> GetRolesAsync(string email);
    }
}