namespace Shared.Wrapper
{
    public class Result : IResult
    {
        public List<string> Messages { get; set; } = new List<string>();
        public bool Succeeded { get; set; }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <returns>The <see cref="Succeeded"/> property value</returns>
        public static IResult Fail()
        {
            return new Result { Succeeded = false };
        }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <param name="message">The error message to add</param>
        /// <returns>The <see cref="Succeeded"/> and <see cref="Messages"/> property values</returns>
        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <param name="messages">The error messages to add</param>
        /// <returns>The <see cref="Succeeded"/> and <see cref="Messages"/> property values</returns>
        public static IResult Fail(List<string> messages)
        {
            return new Result { Succeeded = false, Messages = messages };
        }

        /// <summary>
        /// Successful result
        /// </summary>
        /// <returns>The <see cref="Succeeded"/> property value</returns>
        public static IResult Success()
        {
            return new Result { Succeeded = true };
        }

        /// <summary>
        /// Successful result
        /// </summary>
        /// <param name="message">The success message to add</param>
        /// <returns>The <see cref="Succeeded"/> and <see cref="Messages"/> property values</returns>
        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Messages = new List<string> { message } };
        }
    }

    public class Result<TModel> : IResult<TModel>
    {
        public TModel Data { get; set; }
        public List<string> Messages { get; set; }
        public bool Succeeded { get; set; }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <returns>The <see cref="Succeeded"/> property value</returns>
        public static IResult<TModel> Fail()
        {
            return new Result<TModel> { Succeeded = false };
        }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <param name="message">The error message to add</param>
        /// <returns>The <see cref="Succeeded"/> and <see cref="Messages"/> property values</returns>
        public static IResult<TModel> Fail(string message)
        {
            return new Result<TModel> { Succeeded = false, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <param name="messages">The error messages to add</param>
        /// <returns>The <see cref="Succeeded"/> and <see cref="Messages"/> property values</returns>
        public static IResult<TModel> Fail(List<string> messages)
        {
            return new Result<TModel> { Succeeded = false, Messages = messages };
        }

        /// <summary>
        /// Successful result
        /// </summary>
        /// <returns>The <see cref="Succeeded"/> property value</returns>
        public static IResult<TModel> Success()
        {
            return new Result<TModel> { Succeeded = true };
        }

        /// <summary>
        /// Successful result
        /// </summary>
        /// <param name="message">The success message to add</param>
        /// <returns>The <see cref="Succeeded"/> and <see cref="Messages"/> property values</returns>
        public static IResult<TModel> Success(string message)
        {
            return new Result<TModel> { Succeeded = true, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Successful result
        /// </summary>
        /// <param name="data">The content to add</param>
        /// <returns>The <see cref="Data"/> property value</returns>
        public static IResult<TModel> Success(TModel data)
        {
            return new Result<TModel> { Succeeded = true, Data = data };
        }

        /// <summary>
        /// Successful result
        /// </summary>
        /// <param name="data">The content to add</param>
        /// <param name="message">The success message to add</param>
        /// <returns>The <see cref="Data"/> and <see cref="Messages"/> property values</returns>
        public static Result<TModel> Success(TModel data, string message)
        {
            return new Result<TModel> { Succeeded = true, Data = data, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Successful result
        /// </summary>
        /// <param name="data">The content to add</param>
        /// <param name="messages">The success messages to add</param>
        /// <returns>The <see cref="Data"/> and <see cref="Messages"/> property values</returns>
        public static Result<TModel> Success(TModel data, List<string> messages)
        {
            return new Result<TModel> { Succeeded = true, Data = data, Messages = messages };
        }
    }
}
