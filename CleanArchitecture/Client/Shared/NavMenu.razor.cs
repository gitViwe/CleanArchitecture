namespace Client.Shared
{
    public partial class NavMenu
    {
        private static string GetSwaggerUrl(IConfiguration configuration)
        {
            return configuration["AppConfiguration:ServerUrl"] + "/swagger";
        }
    }
}
