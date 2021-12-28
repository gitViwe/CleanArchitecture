using System.ComponentModel.DataAnnotations;

namespace Core.Request.Identity
{
    /// <summary>
    /// Data Transfer Object for the user password change request
    /// </summary>
    public class ChangePasswordRequest
    {
        [Required]
        public string Password { get; set; }

        [Required,
            Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required,
            Compare(nameof(NewPassword), ErrorMessage = "New Password and Confirm New Password do not match."),
            Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
