namespace Core.DTO
{
    /// <summary>
    /// Data Transfer Object for basic responses
    /// </summary>
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}
