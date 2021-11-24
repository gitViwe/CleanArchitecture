using Core.DTO;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    /// <summary>
    /// This API controller facilitates Claims management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Facilitates dependency injection using constructor injection
        /// </summary>
        public ClaimsController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Gets all claims associated with the specific user
        /// </summary>
        /// <param name="email">The email address belonging to the user</param>
        /// <response code="200">Returns a collection of <see cref="Claim"/> objects</response>
        /// <response code="400">Returns a <see cref="BaseResponse"/> object with error message</response>
        [HttpGet]
        [Route("GetUserClaims")]
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
        /// Add a claim to a specific user
        /// </summary>
        /// <param name="request">The required user and claims information to process the request</param>
        /// <response code="200">Returns a <see cref="BaseResponse"/> object with response message</response>
        /// <response code="400">Returns a <see cref="BaseResponse"/> object with error message</response>
        [HttpPost]
        [Route("AddClaimToUser")]
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
        /// Gets all the claims associated with the specific role
        /// </summary>
        /// <param name="roleName">The name of the Identity Role</param>
        /// <response code="200">Returns a collection of <see cref="Claim"/> objects</response>
        /// <response code="400">Returns a <see cref="BaseResponse"/> object with error message</response>
        [HttpGet]
        [Route("GetRoleClaims")]
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
        /// Add a claim to a specific role
        /// </summary>
        /// <param name="request">The required role and claims information to process the request</param>
        /// <response code="200">Returns a <see cref="BaseResponse"/> object with response message</response>
        /// <response code="400">Returns a <see cref="BaseResponse"/> object with error message</response>
        [HttpPost]
        [Route("AddClaimToRole")]
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
        /// Remove a claim from the user
        /// </summary>
        /// <param name="request">The required user and claims information to process the request</param>
        /// <response code="200">Returns a <see cref="BaseResponse"/> object with response message</response>
        /// <response code="400">Returns a <see cref="BaseResponse"/> object with error message</response>
        [HttpPost]
        [Route("RemoveUserClaim")]
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
        /// Remove a claim from the role
        /// </summary>
        /// <param name="request">The required role and claims information to process the request</param>
        /// <response code="200">Returns a <see cref="BaseResponse"/> object with response message</response>
        /// <response code="400">Returns a <see cref="BaseResponse"/> object with error message</response>
        [HttpPost]
        [Route("RemoveRoleClaim")]
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
