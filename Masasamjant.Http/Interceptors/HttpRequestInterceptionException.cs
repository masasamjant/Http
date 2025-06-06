using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents exception that is thrown when HTTP request is intercepted by an interceptor.
    /// </summary>
    public class HttpRequestInterceptionException: HttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestInterceptionException"/> class with the intercepted HTTP request.
        /// </summary>
        /// <param name="request">The intercepted request.</param>
        public HttpRequestInterceptionException(HttpRequest request)
            : this("The specified HTTP request was intercepted.", request)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestInterceptionException"/> class with a specified error message and the intercepted HTTP request.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="request">The intercepted request.</param>
        public HttpRequestInterceptionException(string message, HttpRequest request)
            : this(message, request, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestInterceptionException"/> class with a specified error message and the intercepted HTTP request.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="request">The intercepted request.</param>
        /// <param name="innerException">The inner exception.</param>
        public HttpRequestInterceptionException(string message, HttpRequest request, Exception? innerException)
            : base(request, message, innerException)
        { }
    }
}
