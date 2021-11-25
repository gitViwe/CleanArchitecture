namespace Core
{
    /// <summary>
    /// Maps the 'JwtConfig' section in AppSettings to this model
    /// </summary>
    public class JwtConfig
    {
        public string Secret { get; set; }
    }
}
