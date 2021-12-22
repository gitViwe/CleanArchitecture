using Client.Infrastructure.Service;
using Shared.Constant.Storage;
using System.Net.Http.Headers;

namespace Client.Infrastructure.Authentication
{
    /// <summary>
    /// Adds the bearer token to the authorization header on every request. Inherits from <see cref="DelegatingHandler"/>
    /// </summary>
    public class ClientAuthenticationHeader : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public ClientAuthenticationHeader(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // authentication scheme must be 'Bearer'
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                // get the saved JWT token
                var savedToken = await _localStorage.GetItemAsync<string>(ClientStorage.Local.AuthToken);

                if (!string.IsNullOrWhiteSpace(savedToken))
                {
                    // use the saved token as the authorization header value
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }

            // process the request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
