namespace Client.Infrastructure.Route
{
    /// <summary>
    /// Provides the request URLs for the authorization controller
    /// </summary>
    public static class AuthorizationEndpoints
    {
        /// <summary>
        /// The end point to login
        /// </summary>
        public const string GetAllRoles = "api/Authorization/GetRoles";

        /// <summary>
        /// The end point to login
        /// </summary>
        public const string CreateRole = "api/Authorization/CreateRole";
    }
}
