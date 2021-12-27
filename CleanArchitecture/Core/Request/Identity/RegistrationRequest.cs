using System.ComponentModel.DataAnnotations;

namespace Core.Request.Identity
{
    /// <summary>
    /// Data Transfer Object for the user registration request
    /// </summary>
    public class RegistrationRequest
    {
        [Required,
            Display(Name = "User name")]
        public string UserName { get; set; }
        [Required,
            EmailAddress(ErrorMessage = "Please enter a valid email address."),
            Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required,
            Display(Name = "Password")]
        public string Password { get; set; }
        [Required,
            Compare("Password", ErrorMessage = "Password and Confirm Password do not match"),
            Display(Name = "Confirm Password")]
        public string PasswordConfirmation { get; set; }
    }
}
