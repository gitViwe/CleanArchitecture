using Core.Request.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.Wrapper;

namespace Infrastructure.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppIdentityUser> _userManager;

        public AccountService(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest request, string userID)
        {
            // check if the user exists
            var user = await _userManager.FindByIdAsync(userID);

            if (user is null)
            {
                return Result.Fail("The user does not exists.");
            }

            // update the user's password
            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);

            if (result.Succeeded)
            {
                return Result.Success("Password updated.");
            }

            return Result.Fail(result.Errors.Select(error => error.Description).ToList());
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest request, string userID)
        {
            // check if the user exists
            var user = await _userManager.FindByIdAsync(userID);

            if (user is null)
            {
                return Result.Fail("The user does not exists.");
            }

            // set property values
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            // update user details
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Result.Success("Profile updated.");
            }

            return Result.Fail(result.Errors.Select(error => error.Description).ToList());
        }
    }
}
