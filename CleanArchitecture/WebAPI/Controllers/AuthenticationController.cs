using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Mime;
using Core.Response;
using Core.Request;
using Core.Configuration;
using Shared.Utility;
using Infrastructure.Service;
using Shared.Wrapper;

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

        /// <summary>
        /// Facilitates dependency injection using constructor injection
        /// </summary>
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Use this endpoint to registers a new user on the system
        /// </summary>
        /// <param name="registrationRequest">This is the required user information to register the user</param>
        /// <response code="200">Returns a model with the token and refresh token</response>
        /// <response code="400">Returns a model with a collection of errors</response>
        [HttpPost]
        [Route(nameof(Register))]
        [ProducesResponseType(typeof(Result<AuthenticationResponse>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authenticationService.RegisterUserAsync(registrationRequest));
            }

            // model validation failed
            return BadRequest(Result.Fail("Registration could not be processed."));
        }

        /// <summary>
        /// Use this endpoint to login an existing user
        /// </summary>
        /// <param name="loginRequest">This is the required user information to login the user</param>
        /// <response code="200">Returns a model with the token and refresh token</response>
        /// <response code="400">Returns a model with a collection of errors</response>
        [HttpPost]
        [Route(nameof(Login))]
        [ProducesResponseType(typeof(Result<AuthenticationResponse>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authenticationService.LoginUserAsync(loginRequest));
            }

            // model validation failed
            return BadRequest(Result.Fail("Login details invalid."));
        }

        /// <summary>
        /// Use this endpoint to request a new token if the current token is invalid
        /// </summary>
        /// <param name="tokenRequest">This is required user information to verify the user and issue a new token</param>
        /// <response code="200">Returns a model with the token and refresh token</response>
        /// <response code="400">Returns a model with a collection of errors</response>
        [HttpPost]
        [Route(nameof(RefreshToken))]
        [ProducesResponseType(typeof(Result<AuthenticationResponse>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authenticationService.RefreshUserTokenAsync(tokenRequest));
            }
            // model validation failed
            return BadRequest(Result.Fail("Token refresh could not be processed."));
        }
    }
}
