using System.ComponentModel.DataAnnotations;

namespace Core.Request.Identity
{
    /// <summary>
    /// Data Transfer Object for role requests
    /// </summary>
    public class RoleRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
