namespace Shared.Constant.Message
{
    /// <summary>
    /// Provides error messages for authorization errors
    /// </summary>
    public static class AuthorizationError
    {
        /// <summary>
        /// Token has expired
        /// </summary>
        public const string ExpiredToken = "The Token has expired.";

        /// <summary>
        /// Not authorized to access
        /// </summary>
        public const string Unauthorized = "You are not Authorized.";

        /// <summary>
        /// Unhandled server error
        /// </summary>
        public const string InternalServerError = "An unhandled error has occurred.";

        /// <summary>
        /// Not authorized to access
        /// </summary>
        public const string Forbidden = "You are not authorized to access this resource.";
    }
}
