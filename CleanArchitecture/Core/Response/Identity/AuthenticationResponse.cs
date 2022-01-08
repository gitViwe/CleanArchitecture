namespace Core.Response.Identity
{
    /// <summary>
    /// Data Transfer Object for authentication responses
    /// </summary>
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
