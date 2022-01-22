using Client.Infrastructure.Manager.Authorization;
using Core.Request.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Wrapper;

namespace Client.Pages.Identity.RolePartial
{
    public partial class RoleDialog
    {
        [Parameter] public RoleRequest Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Inject] private IRoleManager RoleManager { get; set; }
        bool _processing;
        Severity _severity = Severity.Error;
        IResult _result;

        /// <summary>
        /// Processes a request to create a role when form validation passes
        /// </summary>
        public async Task SubmitAsync()
        {
            _processing = true;

            // if the role ID is not provided
            if (string.IsNullOrWhiteSpace(Model.ID))
            {
                // call the create method
                _result = await RoleManager.CreateAsync(Model);
            }
            else
            {
                // else call the update method
                _result = await RoleManager.UpdateAsync(Model);
            }

            if (_result.Succeeded)
            {
                _severity = Severity.Success;
                MudDialog.Close();
            }

            foreach (var message in _result.Messages)
            {
                _snackBar.Add(message, _severity);
            }

            _processing = false;
        }
    }
}
