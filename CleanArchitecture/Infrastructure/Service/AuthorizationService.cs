using Core.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Wrapper;

namespace Infrastructure.Service
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthorizationService(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IResult> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Result<List<IdentityRole>>.Success(roles);
        }

        public async Task<IResult> CreateRoleAsync(string roleName)
        {
            // check if the role already exists
            var exists = await _roleManager.RoleExistsAsync(roleName);

            if (exists)
            {
                return Result.Fail("This role already exists.");
            }

            // create new role
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
            {
                return Result.Success($"The role: {roleName} has been created.");
            }

            return Result.Fail("Could not create role. Please try again.");
        }

        public async Task<IResult> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return Result<List<AppIdentityUser>>.Success(users);
        }

        public async Task<IResult> AddUserToRoleAsync(AuthorizationRequest request)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Fail("The user does not exists.");
            }

            // check if the role exists
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role is null)
            {
                return Result.Fail($"The role: {request.RoleName} does not exist.");
            }

            // check if the user is already assigned to the role
            var isAssigned = await _userManager.IsInRoleAsync(user, request.RoleName);

            if (isAssigned)
            {
                return Result.Fail($"The user: {user.UserName} is already assigned to the role: {request.RoleName}.");
            }

            // assign user to the role
            var result = await _userManager.AddToRoleAsync(user, request.RoleName);

            if (result.Succeeded)
            {
                return Result.Success($"The user: {user.UserName} has been assigned to the role: {request.RoleName}.");
            }

            return Result.Fail("Request could not be complete. Please try again.");
        }

        public async Task<IResult> GetUserRolesAsync(string email)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return Result.Fail("The user does not exist.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return Result<IList<string>>.Success(userRoles);
        }

        public async Task<IResult> RemoveUserFromRoleAsync(AuthorizationRequest request)
        {
            // check if the user exists
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Fail("The user does not exist.");
            }

            // check if the role exists
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role is null)
            {
                return Result.Fail($"The role: {request.RoleName} does not exist.");
            }

            // remove user from the role
            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);

            if (result.Succeeded)
            {
                return Result.Success($"The user: {user.UserName} has been removed from the role: {request.RoleName}.");
            }

            return Result.Fail("Could not be remove user from the role. Please try again.");
        }
    }
}
