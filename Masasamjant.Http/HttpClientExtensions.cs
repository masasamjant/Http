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
            return AddHttpRequestInterceptor(httpClient, interceptor);
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
            return AddHttpRequestInterceptor(httpClient, interceptor);
        }

        /// <summary>
        /// Adds <see cref="ApiKeyHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="IHttpClient"/>.</param>
        /// <param name="apiKeyHeaderName">The name of API key HTTP header.</param>
        /// <param name="apiKeyHeaderValue">The value of API key HTTP header.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        public static IHttpClient AddApiKeyHeaderInterceptor(this IHttpClient httpClient, string? apiKeyHeaderName, string? apiKeyHeaderValue)
        {
            var interceptor = new ApiKeyHeaderInterceptor(apiKeyHeaderName, apiKeyHeaderValue);
            return AddHttpRequestInterceptor(httpClient, interceptor);
        }

        /// <summary>
        /// Adds <see cref="ApiKeyHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="IHttpClient"/>.</param>
        /// <param name="apiKeyHeaderName">The name of API key HTTP header.</param>
        /// <param name="getApiKeyHeaderValue">The delegate to get value of API key HTTP header.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        public static IHttpClient AddApiKeyHeaderInterceptor(this IHttpClient httpClient, string? apiKeyHeaderName, Func<string?> getApiKeyHeaderValue)
        {
            var interceptor = new ApiKeyHeaderInterceptor(apiKeyHeaderName, getApiKeyHeaderValue);
            return AddHttpRequestInterceptor(httpClient, interceptor);
        }

        /// <summary>
        /// Adds <see cref="ApiKeyHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="IHttpClient"/>.</param>
        /// <param name="getApiKeyHeaderName">The delegate to get name of API key HTTP header.</param>
        /// <param name="getApiKeyHeaderValue">The delegate to get value of API key HTTP header.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        public static IHttpClient AddApiKeyHeaderInterceptor(this IHttpClient httpClient, Func<string?> getApiKeyHeaderName, Func<string?> getApiKeyHeaderValue)
        {
            var interceptor = new ApiKeyHeaderInterceptor(getApiKeyHeaderName, getApiKeyHeaderValue);
            return AddHttpRequestInterceptor(httpClient, interceptor);
        }

        /// <summary>
        /// Adds <see cref="AuthenticationTokenHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpClient"/>.</param>
        /// <param name="authTokenHeaderName">The name of authentication token HTTP header.</param>
        /// <param name="authToken">The value of the authentication token HTTP header.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        public static IHttpClient AddAuthenticationTokenHeaderInterceptor(this IHttpClient client, string? authTokenHeaderName, string? authToken)
        {
            var interceptor = new AuthenticationTokenHeaderInterceptor(authTokenHeaderName, authToken);
            return AddHttpRequestInterceptor(client, interceptor);
        }

        /// <summary>
        /// Adds <see cref="AuthenticationTokenHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpClient"/>.</param>
        /// <param name="authTokenHeaderName">The name of authentication token HTTP header.</param>
        /// <param name="getAuthToken">The delegate to get value of the authentication token HTTP header.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        public static IHttpClient AddAuthenticationTokenHeaderInterceptor(this IHttpClient client, string? authTokenHeaderName, Func<string?> getAuthToken)
        {
            var interceptor = new AuthenticationTokenHeaderInterceptor(authTokenHeaderName, getAuthToken);
            return AddHttpRequestInterceptor(client, interceptor);
        }

        /// <summary>
        /// Adds <see cref="AuthenticationTokenHeaderInterceptor"/> to specified <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpClient"/>.</param>
        /// <param name="getAuthTokenHeaderName">The delegate to get name of authentication token HTTP header.</param>
        /// <param name="getAuthToken">The delegate to get value of the authentication token HTTP header.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        public static IHttpClient AddAuthenticationTokenHeaderInterceptor(this IHttpClient client, Func<string?> getAuthTokenHeaderName, Func<string?> getAuthToken)
        {
            var interceptor = new AuthenticationTokenHeaderInterceptor(getAuthTokenHeaderName, getAuthToken);
            return AddHttpRequestInterceptor(client, interceptor);
        }

        private static IHttpClient AddHttpRequestInterceptor(IHttpClient httpClient, HttpRequestInterceptor interceptor)
        {
            httpClient.HttpGetRequestInterceptors?.Add(interceptor);
            httpClient.HttpPostRequestInterceptors?.Add(interceptor);
            return httpClient;
        }
    }
}
