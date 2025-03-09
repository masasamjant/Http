namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents abstract listener of HTTP request execution. Listener is passive component that just get notifies
    /// about HTTP request. It must not change request or not interfere with request processing.
    /// </summary>
    public abstract class HttpClientListener : IHttpClientListener
    {
        /// <summary>
        /// Invoked if exception occurs when executing specified HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="exception">The occurred exception.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public abstract Task OnErrorAsync(HttpRequest request, Exception exception);

        /// <summary>
        /// Invoked after specified HTTP request is executed.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public abstract Task OnExecutedAsync(HttpRequest request);

        /// <summary>
        /// Invoked before specified HTTP request is executed.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public abstract Task OnExecutingAsync(HttpRequest request);
    }
}
