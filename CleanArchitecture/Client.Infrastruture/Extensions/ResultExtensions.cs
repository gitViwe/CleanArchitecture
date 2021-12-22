using Shared.Wrapper;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client.Infrastructure.Extensions
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Process the HTTP response message into the wrapper <see cref="Result"/>
        /// </summary>
        /// <typeparam name="TModel">The data type returned from the response</typeparam>
        /// <param name="response">The HTTP response message from the API</param>
        /// <returns>An HTTP response message as a <see cref="Result"/> model</returns>
        internal static async Task<IResult<TModel>> ToResult<TModel>(this HttpResponseMessage response)
        {
            // get the response as a string
            var responseAsString = await response.Content.ReadAsStringAsync();

            // deserialize response into a 'Result' wrapper class
            var responseObject = JsonSerializer.Deserialize<Result<TModel>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            // return the 'Result' wrapper class
            return responseObject;
        }

        /// <summary>
        /// Process the HTTP response message into the wrapper <see cref="Result"/>
        /// </summary>
        /// <param name="response">The HTTP response message from the API</param>
        /// <returns>An HTTP response message as a <see cref="Result"/> model</returns>
        internal static async Task<IResult> ToResult(this HttpResponseMessage response)
        {
            // get the response as a string
            var responseAsString = await response.Content.ReadAsStringAsync();

            // deserialize response into a 'Result' wrapper class
            var responseObject = JsonSerializer.Deserialize<Result>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            // return the 'Result' wrapper class
            return responseObject;
        }
    }
}
