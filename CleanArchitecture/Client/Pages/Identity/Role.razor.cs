using Client.Pages.Identity.RolePartial;
using Core.Response.Identity;
using MudBlazor;

namespace Client.Pages.Identity
{
    public partial class Role : IDisposable
    {
        private bool dense = true;
        private bool striped = true;
        private bool bordered = false;
        bool _processing;
        private IEnumerable<RoleResponse> _roles = new List<RoleResponse>();

        public void Dispose()
        {
            _interceptorManager.DisposeEvent();
        }

        protected override async Task OnInitializedAsync()
        {
            _interceptorManager.RegisterEvent();

            await GetRolesAsync();
        }

        /// <summary>
        /// Make an API call to load all roles onto the table
        /// </summary>
        private async Task GetRolesAsync()
        {
            _processing = true;

            var result = await _roleManager.GetAllAsync();

            if (result.Succeeded)
            {
                _roles = result.Data;
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

        /// <summary>
        /// Open role modal window
        /// </summary>
        private async Task ShowDialogAsync()
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };

            // show the role dialog window
            var dialogReference = _dialogService.Show<RoleDialog>("Create", options);

            // wait for the user to finish
            await dialogReference.Result;

            // then refresh table
            await GetRolesAsync();
        }
    }
}
