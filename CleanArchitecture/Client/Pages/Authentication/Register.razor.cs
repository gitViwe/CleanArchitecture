using Core.Request.Identity;
using MudBlazor;

namespace Client.Pages.Authentication
{
    public partial class Register
    {
        /// <summary>
        /// The registration view model
        /// </summary>
        public RegistrationRequest Model { get; set; } = new();

        bool _passwordVisibility;
        InputType _passwordInput = InputType.Password;
        string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
        bool _processing;

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
        /// Skips the registration process if authentication state is valid
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // get the current authentication state
            var state = await _authenticationState.GetAuthenticationStateAsync();

            if (state.User.Identity.IsAuthenticated)
            {
                // TODO: redirect user ?
                _snackBar.Add("You are already logged in.", Severity.Success);
            }
        }

        /// <summary>
        /// Processes a registration attempt when form validation passes
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            _processing = true;

            // attempt login
            var result = await _authenticationManager.RegisterAsync(Model);

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

            _processing = false;
        }
    }
}
