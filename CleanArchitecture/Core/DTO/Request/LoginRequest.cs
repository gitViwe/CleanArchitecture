using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    /// <summary>
    /// Data Transfer Object for the user login request
    /// </summary>
    public class LoginRequest
    {
        [Required,
            EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
