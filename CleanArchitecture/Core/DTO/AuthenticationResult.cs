namespace Core.DTO
{
    /// <summary>
    /// Authentication result model
    /// </summary>
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string[] Errors { get; set; }
        public string RefreshToken { get; set; }
    }
}
