namespace Client.Infrastructure.Route
{
    /// <summary>
    /// Provides the request URLs for the authentication controller
    /// </summary>
    public static class AuthenticationEndpoints
    {
        /// <summary>
        /// The end point to login
        /// </summary>
        public const string Login = "api/Authentication/Login";

        /// <summary>
        /// The end point to register
        /// </summary>
        public const string Register = "api/Authentication/Register";
    }
}
