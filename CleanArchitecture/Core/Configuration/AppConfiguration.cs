namespace Core.Configuration
{
    /// <summary>
    /// Maps the <see cref="AppConfiguration"/> section in AppSettings to this model
    /// </summary>
    public class AppConfiguration
    {
        public string Secret { get; set; }
        public string ApplicationUrl { get; set; }
        public string ServerUrl { get; set; }
    }
}
