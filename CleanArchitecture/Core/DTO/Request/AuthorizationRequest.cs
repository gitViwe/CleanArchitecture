using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    /// <summary>
    /// Data Transfer Object for the user - role request
    /// </summary>
    public class AuthorizationRequest
    {
        [Required,
            EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
