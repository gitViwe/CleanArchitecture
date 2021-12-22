using Microsoft.JSInterop;
using System.Text.Json;

namespace Client.Infrastructure.Service
{
    public class LocalStorageService : ILocalStorageService
    {
        private IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<TData> GetItemAsync<TData>(string key)
        {
            // run a JavaScript function to get item based on the 'key'
            var jsonResult = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (jsonResult is null)
            {
                // return default value of the object type
                return default(TData);
            }

            // deserialize JSON string to object type
            return JsonSerializer.Deserialize<TData>(jsonResult);
        }

        public async Task SetItemAsync<TData>(string key, TData data)
        {
            // run a JavaScript function to save item
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize<TData>(data));
        }

        public async Task RemoveItemAsync(string key)
        {
            // run a JavaScript function to delete item
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
