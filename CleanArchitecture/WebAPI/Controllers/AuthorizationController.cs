using Core.DTO;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Facilitate dependency injection using constructor injection
        /// </summary>
        public AuthorizationController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Use this endpoint to get all roles currently stored on the system
        /// </summary>
        /// <response code="200">Returns a collection of all roles</response>
        [HttpGet]
        [Route("GetRoles")]
        [ProducesResponseType(typeof(List<IdentityRole>), 200)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Use this endpoint to create a new role on the system
        /// </summary>
        /// <param name="roleName">This is the name of the Identity Role to create</param>
        /// <response code="200">Returns a success message</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route("CreateRole")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            // check if the role already exists
            var exists = await _roleManager.RoleExistsAsync(roleName);

            if (exists)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { "This role already exists." },
                    Success = false
                });
            }

            // create new role
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
            {
                return Ok(new BaseResponse
                {
                    Message = $"The role: {roleName} has been created.",
                    Success = true
                });
            }

            return BadRequest(new BaseResponse
            {
                Errors = new string[] { "Could not create role. Please try again." },
                Success = false
            });
        }

        /// <summary>
        /// Use this endpoint to get all users currently stored on the system
        /// </summary>
        /// <response code="200">Returns a collection of all the users</response>
        [HttpGet]
        [Route("GetUsers")]
        [ProducesResponseType(typeof(List<AppIdentityUser>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Use this endpoint to assign a role to a specific user
        /// </summary>
        /// <param name="request">This is the required user email and role name</param>
        /// <response code="200">Returns a success message</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route("AddUserToRole")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> AddUserToRole([FromBody] AuthorizationRequest request)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { "The user does not exists." },
                    Success = false
                });
            }

            // check if the role exists
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role is null)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { $"The role: {request.RoleName} does not exist." },
                    Success = false
                });
            }

            // check if the user is already assigned to the role
            var isAssigned = await _userManager.IsInRoleAsync(user, request.RoleName);

            if (isAssigned)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { $"The user: {user.UserName} is already assigned to the role: {request.RoleName}." },
                    Success = false
                });
            }

            // assign user to the role
            var result = await _userManager.AddToRoleAsync(user, request.RoleName);

            if (result.Succeeded)
            {
                return Ok(new BaseResponse
                {
                    Message = $"The user: {user.UserName} has been assigned to the role: {request.RoleName}." ,
                    Success = true
                });
            }

            return BadRequest(new BaseResponse
            {
                Errors = new string[] { "Could not be assigned to the role. Please try again." },
                Success = false
            });
        }

        /// <summary>
        /// Use this endpoint to get all the Identity Roles associated with the specific user
        /// </summary>
        /// <param name="email">This is the email address belonging to the user</param>
        /// <response code="200">Returns a collection of role names</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpGet]
        [Route("GetUserRoles")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { "The user does not exist." },
                    Success = false
                });
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return Ok(userRoles);
        }

        /// <summary>
        /// Use this endpoint to remove a role from a specific user
        /// </summary>
        /// <param name="request">This is required user email and role name</param>
        /// <response code="200">Returns a success message</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route("RemoveUserFromRole")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] AuthorizationRequest request)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { "The user does not exist." },
                    Success = false 
                });
            }

            // check if the role exists
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role is null)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { $"The role: {request.RoleName} does not exist." },
                    Success = false
                });
            }

            // remove user from the role
            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);

            if (result.Succeeded)
            {
                return Ok(new BaseResponse
                {
                    Message = $"The user: {user.UserName} has been removed from the role: {request.RoleName}.",
                    Success = true
                });
            }

            return BadRequest(new BaseResponse
            {
                Errors = new string[] { "Could not be remove user from the role. Please try again." },
                Success = false
            });
        }
    }
}
