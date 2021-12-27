namespace Client.Shared
{
    public partial class NavMenu
    {
        private string GetSwaggerUrl(IConfiguration configuration)
        {
            return configuration["AppConfiguration:ServerUrl"] + "/swagger";
        }
    }
}
