using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    /// <summary>
    /// A custom implementation of the <see cref="IdentityRole"/>
    /// </summary>
    public class AppIdentityRole : IdentityRole
    {
        public AppIdentityRole()
            : base()
        {
            RoleClaims = new HashSet<IdentityRoleClaim<string>>();
        }

        public AppIdentityRole(string roleName, string roleDescription = "")
            : base(roleName)
        {
            Description = roleDescription;
            RoleClaims = new HashSet<IdentityRoleClaim<string>>();
        }

        public string Description { get; set; }
        public virtual ICollection<IdentityRoleClaim<string>> RoleClaims { get; set; }
    }
}
