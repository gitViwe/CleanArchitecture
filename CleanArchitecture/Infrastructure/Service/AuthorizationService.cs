using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuthorizationService(
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IResult> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var response = _mapper.Map<List<RoleResponse>>(roles);
            
            return Result<IEnumerable<RoleResponse>>.Success(response);
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

        public async Task<IResult> UpdateRoleAsync(RoleRequest request)
        {
            // check if the role exists
            var role = await _roleManager.FindByIdAsync(request.ID);

            if (role is null)
            {
                return Result.Fail($"The role: {request.Name} does not exist.");
            }

            // check if the role name is in use
            var existingRole = await _roleManager.FindByNameAsync(request.Name);

            if (existingRole is not null && existingRole.Id != request.ID)
            {
                return Result.Fail($"The role name [{request.Name}] is already in use.");
            }

            // update the name and description
            role.Name = request.Name;
            role.Description = request.Description;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Result.Success($"The role: {request.Name} has been updated.");
            }

            return Result.Fail(result.Errors.Select(item => item.Description).ToList());
        }
    }
}
