using Core.Request.Identity;
using Core.Response.Identity;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Wrapper;
using System.Net.Mime;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Use this API controller to authorize system users
    /// </summary>
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Use this endpoint to registers a new user on the system
        /// </summary>
        /// <param name="registrationRequest">This is the required user information to register the user</param>
        /// <response code="200">Returns a model with the token and refresh token</response>
        [HttpPost]
        [Route(nameof(Register))]
        [ProducesResponseType(typeof(Result<AuthenticationResponse>), 200)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authenticationService.RegisterUserAsync(registrationRequest));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        /// <summary>
        /// Use this endpoint to login an existing user
        /// </summary>
        /// <param name="loginRequest">This is the required user information to login the user</param>
        /// <response code="200">Returns a model with the token and refresh token</response>
        [HttpPost]
        [Route(nameof(Login))]
        [ProducesResponseType(typeof(Result<AuthenticationResponse>), 200)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authenticationService.LoginUserAsync(loginRequest));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        /// <summary>
        /// Use this endpoint to request a new token if the current token is invalid
        /// </summary>
        /// <param name="tokenRequest">This is required user information to verify the user and issue a new token</param>
        /// <response code="200">Returns a model with the token and refresh token</response>
        [HttpPost]
        [Route(nameof(RefreshToken))]
        [ProducesResponseType(typeof(Result<AuthenticationResponse>), 200)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authenticationService.RefreshUserTokenAsync(tokenRequest));
            }

            return Ok(Result.Fail(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList()));
        }
    }
}
