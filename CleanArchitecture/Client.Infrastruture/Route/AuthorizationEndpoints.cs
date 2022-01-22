namespace Client.Infrastructure.Route
{
    /// <summary>
    /// Provides the request URLs for the authorization controller
    /// </summary>
    public static class AuthorizationEndpoints
    {
        /// <summary>
        /// The end point to get all roles
        /// </summary>
        public const string GetAllRoles = "api/Authorization/GetRoles";

        /// <summary>
        /// The end point to create a new role
        /// </summary>
        public const string CreateRole = "api/Authorization/CreateRole";

        /// <summary>
        /// The end point to update a role
        /// </summary>
        public const string UpdateRole = "api/Authorization/UpdateRole";
    }
}
