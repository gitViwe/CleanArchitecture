namespace Core.Configuration
{
    /// <summary>
    /// Maps the 'JwtConfig' section in AppSettings to this model
    /// </summary>
    public class JWTConfiguration
    {
        public string Secret { get; set; }
    }
}
