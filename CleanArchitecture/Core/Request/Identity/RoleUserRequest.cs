using System.ComponentModel.DataAnnotations;

namespace Core.Request.Identity
{
    /// <summary>
    /// Data Transfer Object for the user - role request
    /// </summary>
    public class RoleUserRequest
    {
        [Required,
            EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
