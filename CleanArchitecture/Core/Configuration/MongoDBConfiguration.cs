namespace Core.Configuration
{
    /// <summary>
    /// Maps the <see cref="MongoDBConfiguration"/> section in AppSettings to this model
    /// </summary>
    public class MongoDBConfiguration
    {
        public string MongoDBUrl { get; set; }
        public string DatabaseName { get; set; }
    }
}
