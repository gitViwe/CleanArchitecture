using Core.Response;
using Shared.Wrapper;

namespace Client.Infrastructure.Manager.Demo
{
    /// <summary>
    /// A helper to interface with the weather forecast API
    /// </summary>
    public interface IWeatherForecastManager
    {
        /// <summary>
        /// Gets all the forecasts
        /// </summary>
        /// <returns>A collection of weather forecasts from the API</returns>
        Task<IResult<IEnumerable<WeatherForecast>>> GetForecastAsync();
    }
}
