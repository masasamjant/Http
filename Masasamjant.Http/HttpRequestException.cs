using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents exception thrown when exception occurs at performing HTTP request.
    /// </summary>
    public class HttpRequestException : Exception
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequestException"/> class.
        /// </summary>
        /// <param name="request">The performed HTTP request.</param>
        public HttpRequestException(HttpRequest request)
            : this(request, "The unexpected exception occurred while performing HTTP request.")
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequestException"/> class.
        /// </summary>
        /// <param name="request">The performed HTTP request.</param>
        /// <param name="message">The exception message.</param>
        public HttpRequestException(HttpRequest request, string message)
            : this(request, message, null)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequestException"/> class.
        /// </summary>
        /// <param name="request">The performed HTTP request.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public HttpRequestException(HttpRequest request, string message, Exception? innerException)
            : base(message, innerException)
        {
            HttpRequest = request;
        }

        /// <summary>
        /// Gets the HTTP request where exception occurred.
        /// </summary>
        public HttpRequest HttpRequest { get; }
    }
}
