using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Interceptors;

namespace Masasamjant.Http
{
    /// <summary>
    /// Provides extensions methods to <see cref="IHttpClient"/> interface.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Adds <see cref="HttpRequestIdentifierHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="IHttpClient"/>.</param>
        /// <param name="requestIdentifierHeaderName">The name of request identifier HTTP header.</param>
        /// <returns>A <paramref name="httpClient"/>.</returns>
        /// <exception cref="ArgumentNullException">If value of <paramref name="requestIdentifierHeaderName"/> is empty or only whitespace.</exception>
        public static IHttpClient AddRequestIdentifierHeaderInterceptor(this IHttpClient httpClient, string requestIdentifierHeaderName) 
        {
            var interceptor = new HttpRequestIdentifierHeaderInterceptor(requestIdentifierHeaderName);
            httpClient.HttpGetRequestInterceptors.Add(interceptor);
            httpClient.HttpPostRequestInterceptors.Add(interceptor);
            return httpClient;
        }
    }
}
