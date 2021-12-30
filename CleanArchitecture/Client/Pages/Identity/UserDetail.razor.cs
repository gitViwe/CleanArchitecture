using Client.Extensions;
using Core.Request.Identity;
using MudBlazor;

namespace Client.Pages.Identity
{
    public partial class UserDetail
    {
        /// <summary>
        /// The user detail view model
        /// </summary>
        public UpdateProfileRequest Model { get; set; } = new();

        /// <summary>
        /// The current user's email
        /// </summary>
        public string Email { get; private set; }

        bool _processing;
        Severity _severity;

        protected override async Task OnInitializedAsync()
        {
            var user = await _authenticationManager.CurrentUserAsync();

            Email = user.GetEmail();
            Model.FirstName = user.GetFirstName();
            Model.LastName = user.GetLastName();
            Model.PhoneNumber = user.GetPhoneNumber();
        }

        /// <summary>
        /// Processes a profile update attempt when form validation passes
        /// </summary>
        private async Task SubmitAsync()
        {
            _processing = true;

            // send the update request
            var result = await _accountManager.UpdateProfileAsync(Model);

            if (result.Succeeded)
            {
                _navigationManager.NavigateTo("/account");
                _severity = Severity.Success;
            }
            else
            {
                _severity = Severity.Error;
            }

            foreach (var message in result.Messages)
            {
                _snackBar.Add(message, _severity);
            }

            _processing = false;
        }
    }
}
