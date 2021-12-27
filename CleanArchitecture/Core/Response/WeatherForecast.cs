namespace Core.Response
{
    /// <summary>
    /// Weather forecast model for demo purposes
    /// </summary>
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get { return (TemperatureC * 9 / 5) + 32; } }

        public string Summary { get; set; }
    }
}
