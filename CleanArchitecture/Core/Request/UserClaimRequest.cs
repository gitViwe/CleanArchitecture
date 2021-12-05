using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    /// <summary>
    /// Data Transfer Object for the claims request
    /// </summary>
    public class UserClaimRequest
    {
        [Required,
            EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string ClaimName { get; set; }

        [Required]
        public string ClaimValue { get; set; }
    }
}
