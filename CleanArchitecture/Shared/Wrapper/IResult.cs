namespace Shared.Wrapper
{
    /// <summary>
    /// A Unified return type for the WebAPI
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// The response messages
        /// </summary>
        List<string> Messages { get; set; }

        /// <summary>
        /// Flags whether the request was successful
        /// </summary>
        bool Succeeded { get; set; }
    }

    /// <summary>
    /// Extends on <see cref="IResult"/> to add return content
    /// </summary>
    /// <typeparam name="TModel">The content type returned from the request</typeparam>
    public interface IResult<out TModel> : IResult
    {
        /// <summary>
        /// The data returned from the request
        /// </summary>
        TModel Data { get; }
    }
}
