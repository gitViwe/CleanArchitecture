namespace Client.Infrastructure.Route
{
    /// <summary>
    /// Provides the request URLs for the account controller
    /// </summary>
    public static class AccountEndpoints
    {
        /// <summary>
        /// The end point to change the user's password
        /// </summary>
        public const string ChangePassword = "api/Account/ChangePassword";

        /// <summary>
        /// The end point to update the user's profile
        /// </summary>
        public const string UpdateProfile = "api/Account/UpdateProfile";
    }
}
