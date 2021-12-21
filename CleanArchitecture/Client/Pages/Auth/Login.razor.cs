using Core.Request;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace Client.Pages.Auth
{
    public partial class Login
    {
        /// <summary>
        /// The login view model
        /// </summary>
        LoginRequest _model { get; set; } = new();

        bool PasswordVisibility;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        /// <summary>
        /// The password input field visibility event
        /// </summary>
        void TogglePasswordVisibility()
        {
            if (PasswordVisibility)
            {
                PasswordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        /// <summary>
        /// Skips the login process if authentication state is valid
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // get the current authentication state
            var state = await _authenticationState.GetAuthenticationStateAsync();

            if (state.User.Identity.IsAuthenticated)
            {
                // redirect user to the home page
                _navigationManager.NavigateTo("/");
            }
        }

        /// <summary>
        /// Processes a login attempt when form validation passes
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            // attempt login
            var result = await _authenticationManager.Login(_model);

            if (result.Succeeded == false)
            {
                // display error messages
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            else
            {
                // redirect user to the home page
                _navigationManager.NavigateTo("/");
            }
        }
    }
}
