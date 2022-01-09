using System.ComponentModel.DataAnnotations;

namespace Core.Request.Identity
{
    /// <summary>
    /// Data Transfer Object for role requests
    /// </summary>
    public class RoleRequest
    {
        [Required,
            Display(Name = "Role Name")]
        public string Name { get; set; }

        [Required,
            Display(Name = "Role Description")]
        public string Description { get; set; }
    }
}
