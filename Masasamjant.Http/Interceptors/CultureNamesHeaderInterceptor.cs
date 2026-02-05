using Masasamjant.Http.Abstractions;
using System.Globalization;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents <see cref="HttpRequestInterceptor"/> that adds names of <see cref="CultureInfo.CurrentCulture"/> and <see cref="CultureInfo.CurrentUICulture"/> into HTTP headers.
    /// </summary>
    public sealed class CultureNamesHeaderInterceptor : HttpHeaderRequestInterceptor
    {
        /// <summary>
        /// Initializes new instance of the <see cref="CultureNamesHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="currentCultureHeaderName">The name of current culture HTTP header.</param>
        /// <param name="currentUICultureHeaderName">The name of current UI culture HTTP header.</param>
        /// <exception cref="ArgumentException">If value of <paramref name="currentCultureHeaderName"/> or <paramref name="currentUICultureHeaderName"/> contains invalid character for HTTP header name.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">If value of <paramref name="currentCultureHeaderName"/> or <paramref name="currentUICultureHeaderName"/> is too long.</exception>
        /// <remarks>
        /// If <paramref name="currentCultureHeaderName"/> is <c>null</c>, empty or only whitespace, then name of <see cref="CultureInfo.CurrentCulture"/> is not added.
        /// If <paramref name="currentUICultureHeaderName"/> is <c>null</c>, empty or only whitespace, then name of <see cref="CultureInfo.CurrentUICulture"/> is not added.
        /// If <paramref name="currentCultureHeaderName"/> and <paramref name="currentUICultureHeaderName"/> are same, then only name of <see cref="CultureInfo.CurrentCulture"/> is added.
        /// </remarks>
        public CultureNamesHeaderInterceptor(string? currentCultureHeaderName, string? currentUICultureHeaderName)
        {
            if (!string.IsNullOrWhiteSpace(currentCultureHeaderName))
                HttpHeaderValidator.ValidateHeaderName(currentCultureHeaderName);
            if (!string.IsNullOrWhiteSpace(currentUICultureHeaderName))
                HttpHeaderValidator.ValidateHeaderName(currentUICultureHeaderName);
            CurrentCultureHeaderName = currentCultureHeaderName;
            CurrentUICultureHeaderName = currentUICultureHeaderName;
        }

        /// <summary>
        /// Gets the name of current culture HTTP header.
        /// </summary>
        public string? CurrentCultureHeaderName { get; }

        /// <summary>
        /// Gets the name of current UI culture HTTP header.
        /// </summary>
        public string? CurrentUICultureHeaderName { get; }

        /// <summary>
        /// Intercepts specified <see cref="HttpGetRequest"/> before it it send and appends culture name headers.
        /// </summary>
        /// <param name="request">The <see cref="HttpGetRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request)
        {
            return Task.FromResult(AddCultureNameHeaders(request));
        }

        /// <summary>
        /// Intercepts specified <see cref="HttpPostRequest"/> before it it send and appends culture name headers.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
        {
            return Task.FromResult(AddCultureNameHeaders(request));
        }

        private HttpRequestInterception AddCultureNameHeaders(HttpRequest request)
        {
            var interception = AddCurrentCultureNameHeader(request);
            if (interception.Result != HttpRequestInterceptionResult.Continue)
                return interception;
            return AddCurrentUICultureNameHeader(request);
        }

        private HttpRequestInterception AddCurrentCultureNameHeader(HttpRequest request)
        {
            return AddHttpHeader(request, CurrentCultureHeaderName, CultureInfo.CurrentCulture.Name);
        }

        private HttpRequestInterception AddCurrentUICultureNameHeader(HttpRequest request)
        {
            if (!string.IsNullOrWhiteSpace(CurrentUICultureHeaderName) &&
                !string.Equals(CurrentUICultureHeaderName, CurrentCultureHeaderName, StringComparison.Ordinal))
                return AddHttpHeader(request, CurrentUICultureHeaderName, CultureInfo.CurrentUICulture.Name);
        
            return HttpRequestInterception.Continue;
        }
    }
}
