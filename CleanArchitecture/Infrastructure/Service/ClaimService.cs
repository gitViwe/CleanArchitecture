using Core.Request.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.Constant.Message;
using Shared.Wrapper;
using System.Security.Claims;

namespace Infrastructure.Service
{
    public class ClaimService : IClaimService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimService(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IResult> GetUserClaimsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Fail(ValidationError.Required(nameof(email)));
            }

            // check if the user exists
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return Result.Fail("The user does not exist.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            return Result<IList<Claim>>.Success(userClaims);
        }

        public async Task<IResult> AddClaimToUserAsync(UserClaimRequest request)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Fail("The user does not exist.");
            }

            // create new claim using the name and value
            var userClaim = new Claim(request.ClaimName, request.ClaimValue);

            // associate claim with the user
            var result = await _userManager.AddClaimAsync(user, userClaim);

            if (result.Succeeded)
            {
                return Result.Success("The claim has been associated with the user.");
            }

            return Result.Fail("Request could not be complete. Please try again.");
        }

        public async Task<IResult> GetRoleClaimsAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return Result.Fail(ValidationError.Required(nameof(roleName)));
            }

            // get the identity role using the role name
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
            {
                return Result.Fail($"The role: {roleName} does not exist.");
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            return Result<IList<Claim>>.Success(roleClaims);
        }

        public async Task<IResult> AddClaimToRoleAsync(RoleClaimRequest request)
        {
            // get the identity role using the role name
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role is null)
            {
                return Result.Fail($"The role: {request.RoleName} does not exist.");
            }

            // create new claim using the name and value
            var roleClaim = new Claim(request.ClaimName, request.ClaimValue);

            // associate claim with the user
            var result = await _roleManager.AddClaimAsync(role, roleClaim);

            if (result.Succeeded)
            {
                return Result.Success("The claim has been associated with the role.");
            }

            return Result.Fail("Request could not be complete. Please try again.");
        }

        public async Task<IResult> RemoveUserClaimAsync(UserClaimRequest request)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Fail("The user does not exist.");
            }

            // get all the user claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims is null)
            {
                return Result.Fail("The user does not have any claims.");
            }

            // get the specific user claim
            var claim = userClaims.FirstOrDefault(claim => claim.Type == request.ClaimName && claim.Value == request.ClaimValue);

            if (claim is null)
            {
                return Result.Fail("The user does not have this claim.");
            }

            var result = await _userManager.RemoveClaimAsync(user, claim);

            if (result.Succeeded)
            {
                return Result.Success("The claim has been removed from the user.");
            }

            return Result.Fail("Request could not be complete. Please try again.");
        }

        public async Task<IResult> RemoveRoleClaimAsync(RoleClaimRequest request)
        {
            // check if the user exists
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role is null)
            {
                return Result.Fail("The user does not exist.");
            }

            // get all the user claims
            var roleClaims = await _roleManager.GetClaimsAsync(role);

            if (roleClaims is null)
            {
                return Result.Fail("The role does not have any claims.");
            }

            // get the specific user claim
            var claim = roleClaims.FirstOrDefault(claim => claim.Type == request.ClaimName && claim.Value == request.ClaimValue);

            if (claim is null)
            {
                return Result.Fail("The role does not have this claim.");
            }

            var result = await _roleManager.RemoveClaimAsync(role, claim);

            if (result.Succeeded)
            {
                return Result.Success("The claim has been removed from the role.");
            }

            return Result.Fail("Request could not be complete. Please try again.");
        }
    }
}
