using Core.Request.Identity;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Wrapper;
using System.Net.Mime;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Use this API controller to manage system user profiles
    /// </summary>
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Use this endpoint to update the user's password
        /// </summary>
        /// <param name="request">This is the required user information to change the password</param>
        /// <response code="200">Returns a success message</response>
        [HttpPut]
        [Route(nameof(ChangePassword))]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                var userID = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Ok(await _accountService.ChangePasswordAsync(request, userID));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        /// <summary>
        /// Use this endpoint to update the user's profile details
        /// </summary>
        /// <param name="request">This is the required user information to update the profile</param>
        /// <response code="200">Returns a success message</response>
        [HttpPut]
        [Route(nameof(UpdateProfile))]
        [ProducesResponseType(typeof(Result),200)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            if (ModelState.IsValid)
            {
                var userID = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Ok(await _accountService.UpdateProfileAsync(request, userID));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }
    }
}
