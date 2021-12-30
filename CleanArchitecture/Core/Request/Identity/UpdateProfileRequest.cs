using System.ComponentModel.DataAnnotations;

namespace Core.Request.Identity
{
    /// <summary>
    /// Data Transfer Object for the user profile update request
    /// </summary>
    public class UpdateProfileRequest
    {
        [Required,
            Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required,
            Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
