using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    /// <summary>
    /// A custom implementation of the <see cref="IdentityUser"/>
    /// </summary>
    public class AppIdentityUser : IdentityUser
    {
        public AppIdentityUser()
        {
            // generate a new global unique identifier
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public bool IsActive { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
