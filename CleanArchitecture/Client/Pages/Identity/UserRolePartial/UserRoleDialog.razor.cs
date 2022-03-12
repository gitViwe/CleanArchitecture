using Client.Infrastructure.Manager.Authorization;
using Core.Response.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages.Identity.UserRolePartial
{
    public partial class UserRoleDialog
    {
        [Parameter] public UserResponse User { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Inject] public IUserManager UserManager { get; set; }
        [Inject] public IRoleManager RoleManager { get; set; }
        private IEnumerable<string> _userRoles;

        protected override async Task OnInitializedAsync()
        {
            var response = await UserManager.GetRolesAsync(User.Email);

            if (response.Succeeded)
            {
                _userRoles = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Warning);
                }
            }
        }
    }
}
