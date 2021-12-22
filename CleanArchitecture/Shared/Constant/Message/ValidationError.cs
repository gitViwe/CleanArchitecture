namespace Shared.Constant.Message
{
    /// <summary>
    /// Provides error messages for validation errors
    /// </summary>
    public static class ValidationError
    {
        /// <summary>
        /// Required parameter is empty
        /// </summary>
        public static string Required(string parameterName)
        {
            return "Please provide a value for the required parameter: " + parameterName;
        }
    }
}
