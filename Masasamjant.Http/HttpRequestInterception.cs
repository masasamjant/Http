namespace Masasamjant.Http
{
    /// <summary>
    /// Represents HTTP request interception result and behavior.
    /// </summary>
    public sealed class HttpRequestInterception
    {
        /// <summary>
        /// Gets the result of HTTP request interception.
        /// </summary>
        public HttpRequestInterceptionResult Result { get; }

        /// <summary>
        /// Gets the behavior of canceling the request after interception.
        /// </summary>
        public HttpRequestInterceptionCancelBehavior CancelBehavior { get; }

        /// <summary>
        /// Gets the reason for canceling the request after interception.
        /// </summary>
        public string? CancelReason { get; }

        /// <summary>
        /// Gets whether the request should be canceled after interception.
        /// </summary>
        public bool CancelRequest
            => Result == HttpRequestInterceptionResult.Cancel;

        /// <summary>
        /// Gets whether the request should be canceled and an exception should be thrown after interception.
        /// </summary>
        public bool ThrowCancelException
            => CancelRequest && CancelBehavior == HttpRequestInterceptionCancelBehavior.Throw;

        /// <summary>
        /// Gets a <see cref="HttpRequestInterception"/> that indicates the request should continue after interception.
        /// </summary>
        public static HttpRequestInterception Continue { get; } = new(HttpRequestInterceptionResult.Continue, HttpRequestInterceptionCancelBehavior.Return, null);

        /// <summary>
        /// Creates a new <see cref="HttpRequestInterception"/> that indicates the request should be canceled after interception.
        /// </summary>
        /// <param name="cancelBehavior">The <see cref="HttpRequestInterceptionCancelBehavior"/>.</param>
        /// <param name="cancelReason">The cancel reason.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> that indicates the request should be canceled.</returns>
        public static HttpRequestInterception Cancel(HttpRequestInterceptionCancelBehavior cancelBehavior, string? cancelReason) 
            => new (HttpRequestInterceptionResult.Cancel, cancelBehavior, cancelReason);

        private HttpRequestInterception(HttpRequestInterceptionResult result, HttpRequestInterceptionCancelBehavior cancelBehavior, string? cancelReason)
        {
            Result = result;
            CancelBehavior = cancelBehavior;
            CancelReason = cancelReason;
        }
    }
}
