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

        /// <summary>
        /// Adds <see cref="CultureNamesHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="IHttpClient"/>.</param>
        /// <param name="currentCultureHeaderName">The name of current culture HTTP header.</param>
        /// <param name="currentUICultureHeaderName">The name of current UI culture HTTP header.</param>
        /// <returns>A <paramref name="httpClient"/>.</returns>
        /// <remarks>
        /// If <paramref name="currentCultureHeaderName"/> is <c>null</c>, empty or only whitespace, then name of <see cref="CultureInfo.CurrentCulture"/> is not added.
        /// If <paramref name="currentUICultureHeaderName"/> is <c>null</c>, empty or only whitespace, then name of <see cref="CultureInfo.CurrentUICulture"/> is not added.
        /// If <paramref name="currentCultureHeaderName"/> and <paramref name="currentUICultureHeaderName"/> are same, then only name of <see cref="CultureInfo.CurrentUICulture"/> is added.
        /// </remarks>
        public static IHttpClient AddCultureNamesHeaderInterceptor(this IHttpClient httpClient, string? currentCultureHeaderName, string? currentUICultureHeaderName)
        {
            var interceptor = new CultureNamesHeaderInterceptor(currentCultureHeaderName, currentUICultureHeaderName);
            httpClient.HttpGetRequestInterceptors.Add(interceptor);
            httpClient.HttpPostRequestInterceptors.Add(interceptor);
            return httpClient;
        }
    }
}
