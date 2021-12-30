using Toolbelt.Blazor;

namespace Client.Infrastructure.Manager.Interceptor
{
    /// <summary>
    /// Intercepts HTTP requests to attempt a token refresh
    /// </summary>
    public interface IHttpInterceptorManager
    {
        /// <summary>
        /// We remove the 'InterceptBeforeHttpAsync' event subscription from the event handler.
        /// </summary>
        void DisposeEvent();

        /// <summary>
        /// The event to fire before an HTTP request is sent.<br/>
        /// This is to refresh the JWT token.
        /// </summary>
        Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

        /// <summary>
        /// Registering an event to the BeforeSendAsync event handler.<br/>
        /// This means, before the HTTP request is sent, it is intercepted and the 'InterceptBeforeHttpAsync' event is fired.
        /// </summary>
        void RegisterEvent();
    }
}