namespace Shared.Constant.Storage
{
    /// <summary>
    /// Provides the keys to access client storage item values
    /// </summary>
    public static class ClientStorage
    {
        /// <summary>
        /// Keys to items stored in the browser's local storage
        /// </summary>
        public static class Local
        {
            /// <summary>
            /// The key to the JWT token
            /// </summary>
            public const string AuthToken = "authToken";

            /// <summary>
            /// The key to the JWT refresh token
            /// </summary>
            public const string RefreshToken = "refreshToken";
        }
    }
}
