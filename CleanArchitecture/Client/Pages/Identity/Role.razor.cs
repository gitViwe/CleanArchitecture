using Client.Infrastructure.Manager.Authorization;
using Client.Pages.Identity.RolePartial;
using Core.Request.Identity;
using Core.Response.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages.Identity
{
    public partial class Role : IDisposable
    {
        private bool dense = true;
        private bool striped = true;
        private bool bordered = false;
        private bool _processing;
        private IEnumerable<RoleResponse> _roles = new List<RoleResponse>();
        [Inject] IRoleManager RoleManager { get; set; }

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

            var result = await RoleManager.GetAllAsync();

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
        private async Task ShowCreateDialogAsync()
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };

            // show the role dialog window
            var dialogReference = _dialogService.Show<RoleDialog>("Create", options);

            // wait for the user to finish
            await dialogReference.Result;

            // then refresh table
            await GetRolesAsync();
        }

        private async Task ShowEditDialogAsync(RoleResponse role)
        {
            // a container for the parameters to pass through to the dialog
            var parameters = new DialogParameters();

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };

            // create the model to pass through to the dialog
            var model = new RoleRequest()
            {
                ID = role.Id,
                Name = role.Name,
                Description = role.Description,
            };

            // add model as a parameter value
            parameters.Add("Model", model);

            // show the role dialog window
            var dialogReference = _dialogService.Show<RoleDialog>("Edit", parameters, options);

            // wait for the user to finish
            await dialogReference.Result;

            // then refresh table
            await GetRolesAsync();
        }
    }
}
