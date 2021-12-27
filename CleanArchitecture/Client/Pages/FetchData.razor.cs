using Core.Response;
using MudBlazor;

namespace Client.Pages
{
    public partial class FetchData : IDisposable
    {
        private IEnumerable<WeatherForecast> forecasts = new List<WeatherForecast>();
        private bool dense = true;
        private bool striped = true;
        private bool bordered = true;
        bool _processing;

        public void Dispose()
        {
            _interceptorManager.DisposeEvent();
        }

        protected override async Task OnInitializedAsync()
        {
            _interceptorManager.RegisterEvent();

            _processing = true;

            var result = await _weatherForecastManager.GetForecastAsync();

            await Task.Delay(1500);

            if (result.Succeeded)
            {
                forecasts = result.Data;
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Warning);
                }
            }
            _processing = false;
        }
    }
}
