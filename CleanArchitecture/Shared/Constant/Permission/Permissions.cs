using System.ComponentModel;

namespace Shared.Constant.Permission
{
    /// <summary>
    /// Defines the permission values used by the 'Permission' claims policy
    /// </summary>
    public static class Permissions
    {
        [DisplayName("Forecast")]
        [Description("Forecast Permissions")]
        public static class Forecast
        {
            public const string View = "Permissions.Forecast.View";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
        }

        [DisplayName("Role Claims")]
        [Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }
    }
}
