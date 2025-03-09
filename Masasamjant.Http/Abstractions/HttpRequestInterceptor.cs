namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents base implementation of HTTP request interceptor that intercepts both <see cref="HttpGetRequest"/> and <see cref="HttpPostRequest"/> requests. 
    /// Interceptor is active component that can alter HTTP request or interfere with request processing.
    /// </summary>
    public abstract class HttpRequestInterceptor : IHttpGetRequestInterceptor, IHttpPostRequestInterceptor
    {
        /// <summary>
        /// Intercepts specified <see cref="HttpGetRequest"/> before it it send. This gives interceptor
        /// opportunity to change request or continue or cancel request processing based on result of interception.
        /// </summary>
        /// <param name="request">The <see cref="HttpGetRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public abstract Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request);

        /// <summary>
        /// Intercepts specified <see cref="HttpPostRequest"/> before it it send. This gives interceptor
        /// opportunity to change request or continue or cancel request processing based on result of interception.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public abstract Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request);
    }
}
