namespace Shared.Constant
{
    /// <summary>
    /// Provides the keys to get the values from AppSettings
    /// </summary>
    public static class ApplicationConstant
    {
        /// <summary>
        /// Map to the "Secret" value in the "AppConfiguration" section
        /// </summary>
        public const string Secret = "AppConfiguration:Secret";

        /// <summary>
        /// Map to the "ApplicationUrl" value in the "AppConfiguration" section
        /// </summary>
        public const string ApplicationUrl = "AppConfiguration:ApplicationUrl";

        /// <summary>
        /// Map to the "SQLite" value in the "ConnectionStrings" section
        /// </summary>
        public const string SQLite = "SQLite";

        /// <summary>
        /// Map to the "SQL" value in the "ConnectionStrings" section
        /// </summary>
        public const string SQL = "SQL";

        /// <summary>
        /// Map to the "MongoDB" value in the "ConnectionStrings" section
        /// </summary>
        public const string MongoDB = "MongoDB";

        /// <summary>
        /// Map to the "MongoDB" value in the "DatabaseNames" section
        /// </summary>
        public const string MongoDBName = "DatabaseNames:MongoDB";
    }
}
