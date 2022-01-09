using Client.Infrastructure.Manager.Authorization;
using Core.Request.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages.Identity.RolePartial
{
    public partial class RoleDialog
    {
        public RoleRequest Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Inject] private IRoleManager RoleManager { get; set; }
        bool _processing;
        Severity _severity = Severity.Error;

        /// <summary>
        /// Processes a request to create a role when form validation passes
        /// </summary>
        public async Task SubmitAsync()
        {
            _processing = true;

            var result = await RoleManager.CreateAsync(Model);

            if (result.Succeeded)
            {
                _severity = Severity.Success;
                MudDialog.Close();
            }

            foreach (var message in result.Messages)
            {
                _snackBar.Add(message, _severity);
            }

            _processing = false;
        }
    }
}
