namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents abstract <see cref="HttpRequestInterceptor"/> that adds HTTP header to <see cref="HttpRequest"/>.
    /// </summary>
    public abstract class HttpHeaderRequestInterceptor : HttpRequestInterceptor
    {
        /// <summary>
        /// Add HTTP header with specified name and value to headers of specified <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The HTTP request to add header.</param>
        /// <param name="name">The name of HTTP header.</param>
        /// <param name="value">The value of HTTP header.</param>
        /// <returns>A <see cref="HttpRequestInterception"/>.</returns>
        /// <remarks>HTTP header is not added, if <paramref name="name"/> is <c>null</c>, empty or only whitespace.</remarks>
        protected HttpRequestInterception AddHttpHeader(HttpRequest request, string? name, string? value)
            => AddHttpHeader(request, () => name, () => value);

        /// <summary>
        /// Add HTTP header with specified name and value to headers of specified <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The HTTP request to add header.</param>
        /// <param name="getName">The delegate to get name of HTTP header.</param>
        /// <param name="getValue">The delegate to get value of HTTP header.</param>
        /// <returns>A <see cref="HttpRequestInterception"/>.</returns>
        /// <remarks>HTTP header is not added, if name obtained from <paramref name="getName"/> is <c>null</c>, empty or only whitespace.</remarks>
        protected HttpRequestInterception AddHttpHeader(HttpRequest request, Func<string?> getName, Func<string?> getValue)
        {
            try
            {
                var name = getName();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    HttpHeaderValidator.ValidateHeaderName(name);
                    var value = getValue();
                    request.Headers.Add(name, value);
                }

                return HttpRequestInterception.Continue;
            }
            catch (Exception exception)
            {
                return HttpRequestInterception.Cancel(GetHttpRequestInterceptionCancelBehavior(exception), exception.Message);
            }
        }

        /// <summary>
        /// Gets the <see cref="HttpRequestInterceptionCancelBehavior"/> for specified exception. 
        /// Default is <see cref="HttpRequestInterceptionCancelBehavior.Throw"/>.
        /// </summary>
        /// <param name="exception">The occurred exception.</param>
        /// <returns>A <see cref="HttpRequestInterceptionCancelBehavior"/>.</returns>
        protected virtual HttpRequestInterceptionCancelBehavior GetHttpRequestInterceptionCancelBehavior(Exception exception)
            => HttpRequestInterceptionCancelBehavior.Throw;
    }
}
