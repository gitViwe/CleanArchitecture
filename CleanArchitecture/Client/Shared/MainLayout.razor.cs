using Client.Configuration;
using MudBlazor.ThemeManager;
using MudBlazor;

namespace Client.Shared
{
    public partial class MainLayout
    {
        // set custom theme defaults
        private readonly ThemeManagerTheme _themeManager = new()
        {
            Theme = new DefaultTheme(),
            DrawerClipMode = DrawerClipMode.Always,
            FontFamily = "Montserrat",
            DefaultBorderRadius = 6,
            AppBarElevation = 1,
            DrawerElevation = 1
        };

        // specifies the state of the side bar
        bool _drawerOpen = true;

        // toggles the state of the side bar
        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        async Task OnLogout()
        {
            await _authenticationManager.LogoutAsync();
        }
    }
}
