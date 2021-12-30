using Core.Request.Identity;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Wrapper;
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
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        /// <summary>
        /// Use this endpoint to get all claims associated with the specific user
        /// </summary>
        /// <param name="email">This is the email address belonging to the user</param>
        /// <response code="200">Returns a collection of claims</response>
        [HttpGet]
        [Route(nameof(GetUserClaims))]
        [ProducesResponseType(typeof(Result<IList<Claim>>), 200)]
        public async Task<IActionResult> GetUserClaims(string email)
        {
            return Ok(await _claimService.GetUserClaimsAsync(email));
        }

        /// <summary>
        /// Use this endpoint to add a claim to a specific user
        /// </summary>
        /// <param name="request">This is the required user and claims information to process the request</param>
        /// <response code="200">Returns a response message</response>
        /// <response code="400">Returns a collection of error messages</response>
        [HttpPost]
        [Route(nameof(AddClaimToUser))]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<IActionResult> AddClaimToUser(UserClaimRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _claimService.AddClaimToUserAsync(request));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        /// <summary>
        /// Use this endpoint to get all the claims associated with the specific role
        /// </summary>
        /// <param name="roleName">This is the name of the Identity Role</param>
        /// <response code="200">Returns a collection of claims</response>
        [HttpGet]
        [Route(nameof(GetRoleClaims))]
        [ProducesResponseType(typeof(Result<IList<Claim>>), 200)]
        public async Task<IActionResult> GetRoleClaims(string roleName)
        {
            return Ok(await _claimService.GetRoleClaimsAsync(roleName));
        }

        /// <summary>
        /// Use this endpoint to add a claim to a specific role
        /// </summary>
        /// <param name="request">This is the required role and claims information to process the request</param>
        /// <response code="200">Returns a response message</response>
        [HttpPost]
        [Route(nameof(AddClaimToRole))]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> AddClaimToRole(RoleClaimRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _claimService.AddClaimToRoleAsync(request));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        /// <summary>
        /// Use this endpoint to remove a claim from the user
        /// </summary>
        /// <param name="request">This is the required user and claims information to process the request</param>
        /// <response code="200">Returns a response message</response>
        [HttpPost]
        [Route(nameof(RemoveUserClaim))]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> RemoveUserClaim(UserClaimRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _claimService.RemoveUserClaimAsync(request));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        /// <summary>
        /// Use this endpoint to remove a claim from the role
        /// </summary>
        /// <param name="request">This is the required role and claims information to process the request</param>
        /// <response code="200">Returns a response message</response>
        [HttpPost]
        [Route(nameof(RemoveRoleClaim))]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> RemoveRoleClaim(RoleClaimRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _claimService.RemoveRoleClaimAsync(request));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }
    }
}
