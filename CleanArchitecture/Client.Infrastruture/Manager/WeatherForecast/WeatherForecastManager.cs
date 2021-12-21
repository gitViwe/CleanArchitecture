using Client.Infrastructure.Extensions;
using Core.Response;
using Shared.Wrapper;

namespace Client.Infrastructure.Manager.Forecast
{
    public class WeatherForecastManager : IWeatherForecastManager
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<IEnumerable<WeatherForecast>>> GetForecastAsync()
        {
            // make a get request to the API end point
            var response = await _httpClient.GetAsync(Route.WeatherForecastEndpoints.GetForecast);

            // process the response into a collection of 'WeatherForecast' objects
            return await response.ToResult<IEnumerable<WeatherForecast>>();
        }
    }
}
