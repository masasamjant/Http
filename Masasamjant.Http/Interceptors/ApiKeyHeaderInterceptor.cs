using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents <see cref="HttpRequestInterceptor"/> that adds API key HTTP header to HTTP request.
    /// </summary>
    /// <remarks>HTTP header is not added if header name is <c>null</c>, empty or only white-space.</remarks>
    public sealed class ApiKeyHeaderInterceptor : HttpHeaderRequestInterceptor
    {
        private readonly Func<string?> getApiKeyHeaderName;
        private readonly Func<string?> getApiKey;

        /// <summary>
        /// Initializes new instance of the <see cref="ApiKeyHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="apiKeyHeaderName">The name of API key HTTP header.</param>
        /// <param name="apiKey">The API key value.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="apiKeyHeaderName"/> is empty or only whitespace.</exception>
        /// <exception cref="ArgumentException">If value of <paramref name="apiKeyHeaderName"/> contains invalid HTTP header name characters.</exception>
        public ApiKeyHeaderInterceptor(string? apiKeyHeaderName, string? apiKey)
            : this(() => apiKeyHeaderName, () => apiKey)
        {
            if (apiKeyHeaderName != null)
                HttpHeaderValidator.ValidateHeaderName(apiKeyHeaderName);
        }

        /// <summary>
        /// Initializes new instance of the <see cref="ApiKeyHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="apiKeyHeaderName">The name of API key HTTP header.</param>
        /// <param name="getApiKey">The delegate to get API key value.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="apiKeyHeaderName"/> is empty or only whitespace.</exception>
        /// <exception cref="ArgumentException">If value of <paramref name="apiKeyHeaderName"/> contains invalid HTTP header name characters.</exception>
        public ApiKeyHeaderInterceptor(string? apiKeyHeaderName, Func<string?> getApiKey)
            : this(() => apiKeyHeaderName, getApiKey)
        {
            if (apiKeyHeaderName != null)
                HttpHeaderValidator.ValidateHeaderName(apiKeyHeaderName);
        }

        /// <summary>
        /// Initializes new instance of the <see cref="ApiKeyHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="getApiKeyHeaderName">The delegate to get name of API key HTTP header.</param>
        /// <param name="getApiKey">The delegate to get API key value.</param>
        public ApiKeyHeaderInterceptor(Func<string?> getApiKeyHeaderName, Func<string?> getApiKey)
        {
            this.getApiKeyHeaderName = getApiKeyHeaderName;
            this.getApiKey = getApiKey;
        }

        /// <summary>
        /// Intercepts specified <see cref="HttpGetRequest"/> before it it send and appends API key header.
        /// </summary>
        /// <param name="request">The <see cref="HttpGetRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request)
        {
            return Task.FromResult(AddHttpHeader(request, getApiKeyHeaderName, getApiKey));
        }

        /// <summary>
        /// Intercepts specified <see cref="HttpPostRequest"/> before it it send and appends API key header.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
        {
            return Task.FromResult(AddHttpHeader(request, getApiKeyHeaderName, getApiKey));
        }
    }
}
