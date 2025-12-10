using Masasamjant.Http.Abstractions;
using System.Globalization;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents <see cref="HttpRequestInterceptor"/> that adds names of <see cref="CultureInfo.CurrentCulture"/> and <see cref="CultureInfo.CurrentUICulture"/> into HTTP headers.
    /// </summary>
    public sealed class CultureNamesHeaderInterceptor : HttpRequestInterceptor
    {
        /// <summary>
        /// Initializes new instance of the <see cref="CultureNamesHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="currentCultureHeaderName">The name of current culture HTTP header.</param>
        /// <param name="currentUICultureHeaderName">The name of current UI culture HTTP header.</param>
        /// <remarks>
        /// If <paramref name="currentCultureHeaderName"/> is <c>null</c>, empty or only whitespace, then name of <see cref="CultureInfo.CurrentCulture"/> is not added.
        /// If <paramref name="currentUICultureHeaderName"/> is <c>null</c>, empty or only whitespace, then name of <see cref="CultureInfo.CurrentUICulture"/> is not added.
        /// If <paramref name="currentCultureHeaderName"/> and <paramref name="currentUICultureHeaderName"/> are same, then only name of <see cref="CultureInfo.CurrentCulture"/> is added.
        /// </remarks>
        public CultureNamesHeaderInterceptor(string? currentCultureHeaderName, string? currentUICultureHeaderName)
        {
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
            AddCurrentCultureNameHeader(request);
            AddCurrentUICultureNameHeader(request);
            return HttpRequestInterception.Continue;
        }

        private void AddCurrentCultureNameHeader(HttpRequest request)
        {
            if (!string.IsNullOrWhiteSpace(CurrentCultureHeaderName))
                request.Headers.Add(CurrentCultureHeaderName, CultureInfo.CurrentCulture.Name);
        }

        private void AddCurrentUICultureNameHeader(HttpRequest request)
        {
            if (!string.IsNullOrWhiteSpace(CurrentUICultureHeaderName) &&
                !string.Equals(CurrentUICultureHeaderName, CurrentCultureHeaderName, StringComparison.Ordinal))
                request.Headers.Add(CurrentUICultureHeaderName, CultureInfo.CurrentUICulture.Name);
        }
    }
}
