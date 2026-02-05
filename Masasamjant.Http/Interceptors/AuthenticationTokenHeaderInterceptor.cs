using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents <see cref="HttpRequestInterceptor"/> that adds authentication token HTTP header to HTTP request.
    /// </summary>
    /// <remarks>HTTP header is not added if header name is <c>null</c>, empty or only white-space.</remarks>
    public sealed class AuthenticationTokenHeaderInterceptor : HttpHeaderRequestInterceptor
    {
        private readonly Func<string?> getAuthTokenHeaderName;
        private readonly Func<string?> getAuthToken;

        /// <summary>
        /// Initializes new instance of the <see cref="AuthenticationTokenHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="authTokenHeaderName">The name of authentication token HTTP header.</param>
        /// <param name="authToken">The authentication token value.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="authTokenHeaderName"/> is empty or only whitespace.</exception>
        /// <exception cref="ArgumentException">If value of <paramref name="authTokenHeaderName"/> contains invalid HTTP header name characters.</exception>
        public AuthenticationTokenHeaderInterceptor(string? authTokenHeaderName, string? authToken)
            : this(() => authTokenHeaderName, () => authToken)
        {
            if (authTokenHeaderName != null)
                HttpHeaderValidator.ValidateHeaderName(authTokenHeaderName);
        }

        /// <summary>
        /// Initializes new instance of the <see cref="AuthenticationTokenHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="authTokenHeaderName">The name of authentication token HTTP header.</param>
        /// <param name="getAuthToken">The delegate to get authentication token value.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="authTokenHeaderName"/> is empty or only whitespace.</exception>
        /// <exception cref="ArgumentException">If value of <paramref name="authTokenHeaderName"/> contains invalid HTTP header name characters.</exception>
        public AuthenticationTokenHeaderInterceptor(string? authTokenHeaderName, Func<string?> getAuthToken)
            : this(() => authTokenHeaderName, getAuthToken)
        {
            if (authTokenHeaderName != null)
                HttpHeaderValidator.ValidateHeaderName(authTokenHeaderName);
        }

        /// <summary>
        /// Initializes new instance of the <see cref="AuthenticationTokenHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="getAuthTokenHeaderName">The delegate to get name of authentication token HTTP header.</param>
        /// <param name="getAuthToken">The delegate to get authentication token value.</param>
        public AuthenticationTokenHeaderInterceptor(Func<string?> getAuthTokenHeaderName, Func<string?> getAuthToken)
        {
            this.getAuthTokenHeaderName = getAuthTokenHeaderName;
            this.getAuthToken = getAuthToken;
        }

        /// <summary>
        /// Intercepts specified <see cref="HttpGetRequest"/> before it it send and appends authentication token header.
        /// </summary>
        /// <param name="request">The <see cref="HttpGetRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request)
        {
            return Task.FromResult(AddHttpHeader(request, getAuthTokenHeaderName, getAuthToken));
        }

        /// <summary>
        /// Intercepts specified <see cref="HttpPostRequest"/> before it it send and appends authentication token header.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
        {
            return Task.FromResult(AddHttpHeader(request, getAuthTokenHeaderName, getAuthToken));
        }
    }
}
