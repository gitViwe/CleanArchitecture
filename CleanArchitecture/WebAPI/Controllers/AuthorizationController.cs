using Core.Request;
using Core.Response;
using Infrastructure;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Constant.Message;
using Shared.Wrapper;
using System.Net.Mime;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Use this API controller to manage roles and assign users to roles
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Use this endpoint to get all roles currently stored on the system
        /// </summary>
        /// <response code="200">Returns a collection of all roles</response>
        [HttpGet]
        [Route(nameof(GetRoles))]
        [ProducesResponseType(typeof(Result<List<IdentityRole>>), 200)]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _authorizationService.GetRolesAsync());
        }

        /// <summary>
        /// Use this endpoint to create a new role on the system
        /// </summary>
        /// <param name="roleName">This is the name of the Identity Role to create</param>
        /// <response code="200">Returns a success message</response>
        [HttpPost]
        [Route(nameof(CreateRole))]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            return Ok(await _authorizationService.CreateRoleAsync(roleName));
        }

        /// <summary>
        /// Use this endpoint to get all users currently stored on the system
        /// </summary>
        /// <response code="200">Returns a collection of all the users</response>
        [HttpGet]
        [Route(nameof(GetUsers))]
        [ProducesResponseType(typeof(Result<List<AppIdentityUser>>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _authorizationService.GetUsersAsync());
        }

        /// <summary>
        /// Use this endpoint to assign a role to a specific user
        /// </summary>
        /// <param name="request">This is the required user email and role name</param>
        /// <response code="200">Returns a success message</response>
        [HttpPost]
        [Route(nameof(AddUserToRole))]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> AddUserToRole([FromBody] AuthorizationRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authorizationService.AddUserToRoleAsync(request));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        /// <summary>
        /// Use this endpoint to get all the Identity Roles associated with the specific user
        /// </summary>
        /// <param name="email">This is the email address belonging to the user</param>
        /// <response code="200">Returns a collection of role names</response>
        [HttpGet]
        [Route(nameof(GetUserRoles))]
        [ProducesResponseType(typeof(Result<IList<string>>), 200)]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            return Ok(await _authorizationService.GetUserRolesAsync(email)); 
        }

        /// <summary>
        /// Use this endpoint to remove a role from a specific user
        /// </summary>
        /// <param name="request">This is required user email and role name</param>
        /// <response code="200">Returns a success message</response>
        [HttpPost]
        [Route(nameof(RemoveUserFromRole))]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] AuthorizationRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authorizationService.RemoveUserFromRoleAsync(request));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }
    }
}
