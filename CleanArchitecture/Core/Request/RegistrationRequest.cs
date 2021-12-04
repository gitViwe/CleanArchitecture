using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    /// <summary>
    /// Data Transfer Object for the user registration request
    /// </summary>
    public class RegistrationRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required,
            EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
