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

        /// <summary>
        /// The end point to get all users
        /// </summary>
        public const string GetAllUsers = "api/Authorization/GetUsers";

        /// <summary>
        /// The end point to get roles the user belongs to
        /// </summary>
        /// <param name="email">The route parameter required</param>
        /// <returns></returns>
        public static string GetUserRoles(string email)
        {
            return "api/Authorization/GetUserRolesAsync" + email;
        }
    }
}
