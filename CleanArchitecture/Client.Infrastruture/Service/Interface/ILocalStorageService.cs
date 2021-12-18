
namespace Client.Infrastruture.Service
{
    /// <summary>
    /// Allows access to browser local storage
    /// </summary>
    public interface ILocalStorageService
    {
        /// <summary>
        /// Get an object from local storage
        /// </summary>
        /// <typeparam name="TData">The data type of the object</typeparam>
        /// <param name="key">The identifier name for the object</param>
        /// <returns>The object type based on the key provided. Returns the object's default value if unsuccessful.</returns>
        Task<TData> GetItemAsync<TData>(string key);

        /// <summary>
        /// Delete an object from local storage
        /// </summary>
        /// <param name="key">The identifier name for the object</param>
        /// <returns></returns>
        Task RemoveItemAsync(string key);

        /// <summary>
        /// Store an object to local storage
        /// </summary>
        /// <typeparam name="TData">The data type of the object</typeparam>
        /// <param name="key">The identifier name for the object</param>
        /// <param name="data">The object to store</param>
        /// <returns></returns>
        Task SetItemAsync<TData>(string key, TData data);
    }
}