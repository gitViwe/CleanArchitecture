using Core.Configuration;
using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Utility;
using Shared.Wrapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _validationParameters;
        private readonly APIDbContext _dbContext;
        private readonly AppConfiguration _jwtConfig;

        public AuthenticationService(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<AppConfiguration> optionsMonitor,
            TokenValidationParameters validationParameters,
            APIDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validationParameters = validationParameters;
            _dbContext = dbContext;
            // inject the AppSettings 'JwtConfig' section values
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public async Task<IResult> RegisterUserAsync(RegistrationRequest registrationRequest)
        {
            // verify if email is already registered
            var existingUser = await _userManager.FindByEmailAsync(registrationRequest.Email);

            if (existingUser is not null)
            {
                return Result.Fail("This email is already registered.");
            }

            // create identity user object
            var newUser = new AppIdentityUser { Email = registrationRequest.Email, UserName = registrationRequest.UserName };
            // register new user using request details
            var result = await _userManager.CreateAsync(newUser, registrationRequest.Password);

            if (result.Succeeded)
            {
                var newToken = await GenerateJWTToken(newUser);

                return Result<AuthenticationResponse>.Success(newToken);
            }
            // user could not be registered
            else
            {
                // errors that occurred during user registration
                return Result.Fail(result.Errors.Select(error => error.Description).ToList());
            }
        }

        public async Task<IResult> LoginUserAsync(LoginRequest loginRequest)
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
                    var newToken = await GenerateJWTToken(existingUser);

                    return Result<AuthenticationResponse>.Success(newToken);
                }
            }

            return Result.Fail("Login details invalid.");
        }

        public async Task<IResult> RefreshUserTokenAsync(TokenRequest tokenRequest)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                // verify that the token is a JWT token
                var jwtClaims = handler.ValidateToken(tokenRequest.Token, _validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken securityToken)
                {
                    // verify that the token is encrypted with the security algorithm
                    var result = securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result is false)
                    {
                        return Result.Fail("Invalid request.");
                    }
                }

                // verify that the expiry date has not passed
                var expiry = long.Parse(jwtClaims.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = Conversion.UnixTimeStampToDateTime(expiry);
                if (expiryDate > DateTime.UtcNow)
                {
                    return Result.Fail("The token has not yet expired.");
                }

                // verify that the JWT token is on the database
                var storedToken = _dbContext.RefreshTokens.FirstOrDefault(item => item.Token == tokenRequest.RefreshToken);
                if (storedToken is null)
                {
                    return Result.Fail("The token does not exist.");
                }

                // verify that the token is not being used
                if (storedToken.IsUsed)
                {
                    return Result.Fail("The token has already been used.");
                }

                // verify that the token has not been revoked
                if (storedToken.IsRevoked)
                {
                    return Result.Fail("The token has been revoked.");
                }

                // verify the token ID
                var tokenID = jwtClaims.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != tokenID)
                {
                    return Result.Fail("The token is not valid.");
                }

                // update current token and save changes
                storedToken.IsUsed = true;
                _dbContext.RefreshTokens.Update(storedToken);
                await _dbContext.SaveChangesAsync();

                // generate new token
                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                return Result<AuthenticationResponse>.Success(await GenerateJWTToken(user));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return Result.Fail("The token has expired, please login again.");
                }
                return Result.Fail("We encountered an error, please try again.");
            }
        }

        private async Task<AuthenticationResponse> GenerateJWTToken(AppIdentityUser user)
        {
            // create token handler object
            var handler = new JwtSecurityTokenHandler();
            // encode security key
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = await GetAllValidClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
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
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                Token = Conversion.RandomString(35) + Guid.NewGuid()
            };
            // save the refresh token entity in the database
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            // return the authentication result
            return new AuthenticationResponse()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
            };
        }

        private async Task<List<Claim>> GetAllValidClaims(AppIdentityUser user)
        {
            // add required claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // use First name and Last name, if it is empty, then use User name
                new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.FirstName + user.LastName) ? user.UserName : user.FirstName + " " + user.LastName)
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
