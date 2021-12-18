using Microsoft.AspNetCore.Components;
using Shared.Constant.Storage;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client.Infrastruture.Service
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorage;

        public HttpService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorage = localStorage;
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            // create GET request
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            // send request
            return await SendRequestAsync<TResult>(request);
        }

        public async Task<TResult> PostAsync<TResult>(string uri, object value)
        {
            // create POST request
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            // add and serialize content to JSON format
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            // send request
            return await SendRequestAsync<TResult>(request);
        }

        private async Task<TResult> SendRequestAsync<TResult>(HttpRequestMessage request)
        {
            // get the user data from local storage
            var token = await _localStorage.GetItemAsync<string>(ClientStorage.Local.AuthToken);

            // check the request URL
            var isAPIRequest = request.RequestUri.IsAbsoluteUri == false;

            if (string.IsNullOrWhiteSpace(token) == false && isAPIRequest)
            {
                // add JWT authentication header if user is logged in and request is to the API
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // send the request
            using var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // TODO: attempt a login (refresh token) ?
                _navigationManager.NavigateTo("/auth/login");
                // exit code block
                return default;
            }

            if (response.IsSuccessStatusCode == false)
            {
                // TODO: error handling on response ??
            }

            return await response.Content.ReadFromJsonAsync<TResult>();
        }
    }
}
