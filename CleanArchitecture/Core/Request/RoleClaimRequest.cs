using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    /// <summary>
    /// Data Transfer Object for the claims request
    /// </summary>
    public class RoleClaimRequest
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string ClaimName { get; set; }

        [Required]
        public string ClaimValue { get; set; }
    }
}
