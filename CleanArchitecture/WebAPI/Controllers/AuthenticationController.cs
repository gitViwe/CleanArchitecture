using Core.DTO;
using Core;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.Cryptography;

namespace WebAPI.Controllers
{
    /// <summary>
    /// This API controller facilitates user authorization
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _validationParameters;
        private readonly APIDbContext _dbContext;
        private readonly IConversion _conversion;
        private readonly JwtConfig _jwtConfig;

        /// <summary>
        /// Facilitates dependency injection using constructor injection
        /// </summary>
        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters validationParameters,
            APIDbContext dbContext,
            IConversion conversion)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validationParameters = validationParameters;
            _dbContext = dbContext;
            _conversion = conversion;
            // inject the AppSettings 'JwtConfig' section values
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        /// <summary>
        /// Registers a new user on the system
        /// </summary>
        /// <param name="registrationRequest">The required user information to register the user</param>
        /// <response code="200">Returns a <see cref="AuthenticationResult"/> with the token and refresh token</response>
        /// <response code="400">Returns a <see cref="RegistrationResponse"/> with collection of errors</response>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            if (ModelState.IsValid)
            {
                // verify if email is already registered
                var existingUser = await _userManager.FindByEmailAsync(registrationRequest.Email);

                if (existingUser is not null)
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new string[] { "This email is already registered." },
                        Success = false
                    });
                }

                // create identity user object
                var newUser = new IdentityUser { Email = registrationRequest.Email, UserName = registrationRequest.UserName };
                // register new user using request details
                var result = await _userManager.CreateAsync(newUser, registrationRequest.Password);

                if (result.Succeeded)
                {
                    return Ok(await GenerateJWTToken(newUser));
                }
                // user could not be registered
                else
                {
                    return BadRequest(new RegistrationResponse
                    {
                        // errors that occurred during user registration
                        Errors = result.Errors.Select(error => error.Description).ToArray(),
                        Success = false
                    });
                }
            }

            // model validation failed
            return BadRequest(new RegistrationResponse
            {
                Errors = new string[] { "Registration could not be processed." },
                Success = false
            });
        }

        /// <summary>
        /// Login an existing user
        /// </summary>
        /// <param name="loginRequest">The required user information to login the user</param>
        /// <response code="200">Returns a <see cref="AuthenticationResult"/> with the token and refresh token</response>
        /// <response code="400">Returns a <see cref="RegistrationResponse"/> with collection of errors</response>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                // verify if email is registered
                var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);
                // no matching email
                if (existingUser is not null)
                {
                    // verify that the password is valid
                    var isCorrectPassword = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);

                    if (isCorrectPassword)
                    {
                        return Ok(await GenerateJWTToken(existingUser));
                    }
                }
            }

            // model validation failed
            return BadRequest(new RegistrationResponse
            {
                Errors = new string[] { "Login details invalid." },
                Success = false
            });
        }

        /// <summary>
        /// Request a new token if the current token has expired
        /// </summary>
        /// <param name="tokenRequest">The required user information to verify the user and issue a new token</param>
        /// <response code="200">Returns a <see cref="AuthenticationResult"/> with the token and refresh token</response>
        /// <response code="400">Returns a <see cref="RegistrationResponse"/> with collection of errors</response>
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                // verify and generate new token
                var result = await VerifyandGenerateToken(tokenRequest);
                if (result is not null)
                {
                    return Ok(result);
                }
            }
            // model validation failed
            return BadRequest(new RegistrationResponse
            {
                Errors = new string[] { "Token refresh could not be processed." },
                Success = false
            });
        }

        private async Task<AuthenticationResult> GenerateJWTToken(IdentityUser user)
        {
            // create token handler object
            var handler = new JwtSecurityTokenHandler();
            // encode security key
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = await GetAllValidClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            // creates a security token
            var token = handler.CreateToken(tokenDescriptor);
            // serialize the security token
            var jwtToken = handler.WriteToken(token);
            // create refresh token entity
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                Token = _conversion.RandomString(35) + Guid.NewGuid()
            };
            // save the refresh token entity in the database
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            // return the authentication result
            return new AuthenticationResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Success = true
            };
        }

        private async Task<AuthenticationResult> VerifyandGenerateToken(TokenRequest request)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                // verify that the token is a JWT token
                var jwtClaims = handler.ValidateToken(request.Token, _validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken securityToken)
                {
                    // verify that the token is encrypted with the security algorithm
                    var result = securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result is false)
                    {
                        return new AuthenticationResult()
                        {
                            Success = false,
                            Errors = new string[] { "Invalid request." }
                        };
                    }
                }

                // verify that the expiry date has not passed
                var expiry = long.Parse(jwtClaims.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = _conversion.UnixTimeStampToDateTime(expiry);
                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new string[] { "The token has not yet expired." }
                    };
                }

                // verify that the JWT token is on the database
                var storedToken = _dbContext.RefreshTokens.FirstOrDefault(item => item.Token == request.RefreshToken);
                if (storedToken is null)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new string[] { "The token does not exist." }
                    };
                }

                // verify that the token is not being used
                if (storedToken.IsUsed)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new string[] { "The token has already been used." }
                    };
                }

                // verify that the token has not been revoked
                if (storedToken.IsRevoked)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new string[] { "The token has been revoked." }
                    };
                }

                // verify the token ID
                var tokenID = jwtClaims.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != tokenID)
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new string[] { "The token is not valid." }
                    };
                }

                // update current token and save changes
                storedToken.IsUsed = true;
                _dbContext.RefreshTokens.Update(storedToken);
                await _dbContext.SaveChangesAsync();

                // generate new token
                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJWTToken(user);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new string[] { "The token has expired, please login again." }
                    };
                }
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new string[] { "We encountered an error, please try again." }
                };
            }
        }

        private async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
            // add required claims
            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // get claims that are assigned to the user...
            var userClaims = await _userManager.GetClaimsAsync(user);
            // ...and ad them to the claims collection
            claims.AddRange(userClaims);

            // get roles that are assigned to the user
            foreach (var roleName in await _userManager.GetRolesAsync(user))
            {
                // get the identity role using the role name
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role is null)
                {
                    // skip this iteration
                    continue;
                }

                // add the role to the claims collection
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                // get all claims associated with that claim
                foreach (var roleClaim in await _roleManager.GetClaimsAsync(role))
                {
                    claims.Add(roleClaim);
                }
            }

            return claims;
        }
    }
}
