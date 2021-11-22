using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    /// <summary>
    /// Data Transfer Object for the user registration request
    /// </summary>
    public class RegistrationRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required,
            EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
