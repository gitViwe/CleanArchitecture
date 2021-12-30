using Core.Request.Identity;
using MudBlazor;

namespace Client.Pages.Identity
{
    public partial class UserPassword
    {
        /// <summary>
        /// The change password view model
        /// </summary>
        public ChangePasswordRequest Model { get; set; } = new();

        bool _processing;
        Severity _severity;

        bool _passwordVisibility;
        InputType _passwordInput = InputType.Password;
        string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        /// <summary>
        /// The password input field visibility event
        /// </summary>
        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        /// <summary>
        /// Processes a password update attempt when form validation passes
        /// </summary>
        private async Task SubmitAsync()
        {
            _processing = true;

            // send the update request
            var result = await _accountManager.ChangePasswordAsync(Model);

            if (result.Succeeded)
            {
                await _authenticationManager.TryRefreshTokenAsync();
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
