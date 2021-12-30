using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Extensions
{
    /// <summary>
    /// A helper class to easily get the claims values
    /// </summary>
    internal static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the Email value from the claims
        /// </summary>
        internal static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Email).Value;
        }

        /// <summary>
        /// Gets the Name value from the claims
        /// </summary>
        internal static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value;
        }

        /// <summary>
        /// Gets the Surname value from the claims
        /// </summary>
        internal static string GetLastName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(JwtRegisteredClaimNames.FamilyName)?.Value;
        }

        /// <summary>
        /// Gets the MobilePhone value from the claims
        /// </summary>
        internal static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.MobilePhone)?.Value;
        }

        /// <summary>
        /// Gets the NameIdentifier value from the claims
        /// </summary>
        internal static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
