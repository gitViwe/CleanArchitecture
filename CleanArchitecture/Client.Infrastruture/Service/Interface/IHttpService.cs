
namespace Client.Infrastruture.Service
{
    /// <summary>
    /// The HTTP service builds on the .NET Core HttpClient to simplify the code for making HTTP requests from other services
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Sends a GET request to the API
        /// </summary>
        /// <typeparam name="TResult">The type of object expected from the response</typeparam>
        /// <param name="uri">The API endpoint</param>
        /// <returns></returns>
        Task<TResult> GetAsync<TResult>(string uri);

        /// <summary>
        /// Sends a POST request to the API
        /// </summary>
        /// <typeparam name="TResult">The type of object expected from the response</typeparam>
        /// <param name="uri">The API endpoint</param>
        /// <param name="value">The data to send to the API</param>
        /// <returns></returns>
        Task<TResult> PostAsync<TResult>(string uri, object value);
    }
}