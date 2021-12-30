using System.ComponentModel.DataAnnotations;

namespace Core.Request.Identity
{
    /// <summary>
    /// Data Transfer Object for the user login request
    /// </summary>
    public class LoginRequest
    {
        [Required,
            Display(Name = "Email Address"),
            EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required,
            Display(Name = "Password")]
        public string Password { get; set; }
    }
}
