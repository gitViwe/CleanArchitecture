using Core.DTO;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    /// <summary>
    /// This API controller facilitates Claims management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ClaimsController : ControllerBase
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Facilitates dependency injection using constructor injection
        /// </summary>
        public ClaimsController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Use this endpoint to get all claims associated with the specific user
        /// </summary>
        /// <param name="email">This is the email address belonging to the user</param>
        /// <response code="200">Returns a collection of claims</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpGet]
        [Route("GetUserClaims")]
        [ProducesResponseType(typeof(List<Claim>), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> GetUserClaims(string email)
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

            var userClaims = await _userManager.GetClaimsAsync(user);

            return Ok(userClaims);
        }

        /// <summary>
        /// Use this endpoint to add a claim to a specific user
        /// </summary>
        /// <param name="request">This is the required user and claims information to process the request</param>
        /// <response code="200">Returns a response message</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route("AddClaimToUser")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> AddClaimToUser(UserClaimRequest request)
        {
            if (ModelState.IsValid)
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

                // create new claim using the name and value
                var userClaim = new Claim(request.ClaimName, request.ClaimValue);

                // associate claim with the user
                var result = await _userManager.AddClaimAsync(user, userClaim);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        Message = "The claim has been associated with the user.",
                        Success = true
                    });
                } 
            }

            return BadRequest(new BaseResponse
            {
                Errors = new string[] { "Could not associated claim with the user. Please try again." },
                Success = false
            });
        }

        /// <summary>
        /// Use this endpoint to get all the claims associated with the specific role
        /// </summary>
        /// <param name="roleName">This is the name of the Identity Role</param>
        /// <response code="200">Returns a collection of claims</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpGet]
        [Route("GetRoleClaims")]
        [ProducesResponseType(typeof(List<Claim>), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> GetRoleClaims(string roleName)
        {
            // get the identity role using the role name
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
            {
                return BadRequest(new BaseResponse
                {
                    Errors = new string[] { $"The role: {roleName} does not exist." },
                    Success = false
                });
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            return Ok(roleClaims);
        }

        /// <summary>
        /// Use this endpoint to add a claim to a specific role
        /// </summary>
        /// <param name="request">This is the required role and claims information to process the request</param>
        /// <response code="200">Returns a collection of claims</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route("AddClaimToRole")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> AddClaimToRole(RoleClaimRequest request)
        {
            if (ModelState.IsValid)
            {
                // get the identity role using the role name
                var role = await _roleManager.FindByNameAsync(request.RoleName);

                if (role is null)
                {
                    return BadRequest(new BaseResponse
                    {
                        Errors = new string[] { $"The role: {request.RoleName} does not exist." },
                        Success = false
                    });
                }

                // create new claim using the name and value
                var roleClaim = new Claim(request.ClaimName, request.ClaimValue);

                // associate claim with the user
                var result = await _roleManager.AddClaimAsync(role, roleClaim);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        Message = "The claim has been associated with the role.",
                        Success = true
                    });
                } 
            }

            return BadRequest(new BaseResponse
            {
                Errors = new string[] { "Could not associated claim with the role. Please try again." },
                Success = false
            });
        }

        /// <summary>
        /// Use this endpoint to remove a claim from the user
        /// </summary>
        /// <param name="request">This is the required user and claims information to process the request</param>
        /// <response code="200">Returns a collection of claims</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route("RemoveUserClaim")]
        [ProducesResponseType(typeof(List<Claim>), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> RemoveUserClaim(UserClaimRequest request)
        {
            if (ModelState.IsValid)
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

                // get all the user claims
                var userClaims = await _userManager.GetClaimsAsync(user);

                if (userClaims is null)
                {
                    return BadRequest(new BaseResponse
                    {
                        Errors = new string[] { "The user does not have any claims." },
                        Success = false
                    });
                }

                // get the specific user claim
                var claim = userClaims.FirstOrDefault(claim => claim.Type == request.ClaimName && claim.Value == request.ClaimValue);

                if (claim is null)
                {
                    return BadRequest(new BaseResponse
                    {
                        Errors = new string[] { "The user does not have this claim." },
                        Success = false
                    });
                }

                var result = await _userManager.RemoveClaimAsync(user, claim);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        Message = "The claim has been removed from the user.",
                        Success = true
                    });
                }
            }

            return BadRequest(new BaseResponse
            {
                Errors = new string[] { "Could not remove claim from user. Please try again." },
                Success = false
            });
        }

        /// <summary>
        /// Use this endpoint to remove a claim from the role
        /// </summary>
        /// <param name="request">This is the required role and claims information to process the request</param>
        /// <response code="200">Returns a collection of claims</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route("RemoveRoleClaim")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> RemoveRoleClaim(RoleClaimRequest request)
        {
            if (ModelState.IsValid)
            {
                // check if the user exists
                var role = await _roleManager.FindByNameAsync(request.RoleName);

                if (role is null)
                {
                    return BadRequest(new BaseResponse
                    {
                        Errors = new string[] { "The role does not exist." },
                        Success = false
                    });
                }

                // get all the user claims
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                if (roleClaims is null)
                {
                    return BadRequest(new BaseResponse
                    {
                        Errors = new string[] { "The role does not have any claims." },
                        Success = false
                    });
                }

                // get the specific user claim
                var claim = roleClaims.FirstOrDefault(claim => claim.Type == request.ClaimName && claim.Value == request.ClaimValue);

                if (claim is null)
                {
                    return BadRequest(new BaseResponse
                    {
                        Errors = new string[] { "The role does not have this claim." },
                        Success = false
                    });
                }

                var result = await _roleManager.RemoveClaimAsync(role, claim);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        Message = "The claim has been removed from the role.",
                        Success = true
                    });
                }
            }

            return BadRequest(new BaseResponse
            {
                Errors = new string[] { "Could not remove claim from role. Please try again." },
                Success = false
            });
        }
    }
}
