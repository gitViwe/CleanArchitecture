using System.ComponentModel.DataAnnotations;

namespace Core.Response.Identity
{
    /// <summary>
    /// Data Transfer Object for role responses
    /// </summary>
    public class RoleResponse
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
