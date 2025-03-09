namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents generic interceptor of <typeparamref name="TRequest"/> HTTP request. Interceptor is active component that 
    /// can alter HTTP request or interfere with request processing.
    /// </summary>
    /// <typeparam name="TRequest">The type of the HTTP request.</typeparam>
    public interface IHttpRequestInterceptor<TRequest> where TRequest : HttpRequest
    {
        /// <summary>
        /// Intercepts specified <typeparamref name="TRequest"/> HTTP request before it it send. This gives interceptor
        /// opportunity to change request or continue or cancel request processing based on result of interception.
        /// </summary>
        /// <param name="request">The HTTP request to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        Task<HttpRequestInterception> InterceptAsync(TRequest request);
    }
}
