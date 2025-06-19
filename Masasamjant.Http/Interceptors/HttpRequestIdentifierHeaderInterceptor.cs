using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents <see cref="HttpRequestInterceptor"/> that adds <see cref="HttpRequest.Identifier"/> value into HTTP headers.
    /// </summary>
    public sealed class HttpRequestIdentifierHeaderInterceptor : HttpRequestInterceptor
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequestIdentifierHeaderInterceptor"/> class.
        /// </summary>
        /// <param name="requestIdentifierHeaderName">The name of request identifier HTTP header.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="requestIdentifierHeaderName"/> is empty or only whitespace.</exception>
        public HttpRequestIdentifierHeaderInterceptor(string requestIdentifierHeaderName)
        {
            if (string.IsNullOrWhiteSpace(requestIdentifierHeaderName))
                throw new ArgumentNullException(nameof(requestIdentifierHeaderName), "The request identifier HTTP header name is empty or only whitespace.");

            RequestIdentifierHeaderName = requestIdentifierHeaderName;
        }

        /// <summary>
        /// Gets the name of request identifier HTTP header.
        /// </summary>
        public string RequestIdentifierHeaderName { get; }

        /// <summary>
        /// Intercepts specified <see cref="HttpGetRequest"/> before it it send. This gives interceptor
        /// opportunity to change request or continue or cancel request processing based on result of interception.
        /// </summary>
        /// <param name="request">The <see cref="HttpGetRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request)
        {
            AddRequestIdentifierHeader(request);
            return Task.FromResult(HttpRequestInterception.Continue);
        }

        /// <summary>
        /// Intercepts specified <see cref="HttpPostRequest"/> before it it send. This gives interceptor
        /// opportunity to change request or continue or cancel request processing based on result of interception.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
        {
            AddRequestIdentifierHeader(request);
            return Task.FromResult(HttpRequestInterception.Continue);
        }

        private void AddRequestIdentifierHeader(HttpRequest request)
        {
            request.Headers.Add(RequestIdentifierHeaderName, request.Identifier.ToString());
        }
    }
}
