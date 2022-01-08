using Core.Request.Identity;
using Core.Response.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Constant.Message;
using Shared.Wrapper;

namespace Infrastructure.Service
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;

        public AuthorizationService(
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IResult> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            // TODO: Add auto mapper
            var output = new List<RoleResponse>();
            foreach (var item in roles)
            {
                output.Add(new RoleResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                });
            }

            return Result<IEnumerable<RoleResponse>>.Success(output);
        }

        public async Task<IResult> CreateRoleAsync(RoleRequest request)
        {
            // check if the role already exists
            var exists = await _roleManager.RoleExistsAsync(request.Name);

            if (exists)
            {
                return Result.Fail("This role already exists.");
            }

            // create new role
            var result = await _roleManager.CreateAsync(new AppIdentityRole(request.Name, request.Description));

            if (result.Succeeded)
            {
                return Result.Success($"The role: {request.Name} has been created.");
            }

            return Result.Fail("Could not create role. Please try again.");
        }

        public async Task<IResult> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return Result<List<AppIdentityUser>>.Success(users);
        }

        public async Task<IResult> AddUserToRoleAsync(RoleUserRequest request)
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

            var userRoles = await _userManager.GetRolesAsync(user);

            return Result<IList<string>>.Success(userRoles);
        }

        public async Task<IResult> RemoveUserFromRoleAsync(RoleUserRequest request)
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
