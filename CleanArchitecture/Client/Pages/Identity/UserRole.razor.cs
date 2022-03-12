using Client.Infrastructure.Manager.Authorization;
using Core.Response.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages.Identity
{
    public partial class UserRole : IDisposable
    {
        private bool dense = true;
        private bool striped = true;
        private bool bordered = false;
        private bool _processing;
        private IEnumerable<UserResponse> _users = new List<UserResponse>();
        [Inject] IUserManager UserManager { get; set; }

        public void Dispose()
        {
            _interceptorManager.DisposeEvent();
        }

        protected override async Task OnInitializedAsync()
        {
            _interceptorManager.RegisterEvent();

            await GetUsersAsync();
        }

        /// <summary>
        /// Make an API call to load all users onto the table
        /// </summary>
        private async Task GetUsersAsync()
        {
            _processing = true;

            var result = await UserManager.GetAllAsync();

            if (result.Succeeded)
            {
                _users = result.Data;
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Warning);
                }
            }
            _processing = false;
        }
    }
}
