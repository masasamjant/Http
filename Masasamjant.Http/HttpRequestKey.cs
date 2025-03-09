using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents key that can be used to identify HTTP request.
    /// </summary>
    public sealed class HttpRequestKey : IEquatable<HttpRequestKey>
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequestKey"/> class.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        public HttpRequestKey(HttpRequest request)
        {
            RequestUri = request.GetFullRequestUri();
            RequestIdentifier = request.Identifier;
            RequestMethod = request.Method;
        }

        /// <summary>
        /// Gets the full request URI.
        /// </summary>
        public string RequestUri { get; }

        /// <summary>
        /// Gets the request identifier.
        /// </summary>
        public Guid RequestIdentifier { get; }

        /// <summary>
        /// Gets the request method.
        /// </summary>
        public HttpRequestMethod RequestMethod { get; }

        /// <summary>
        /// Check if other <see cref="HttpRequestKey"/> is not <c>null</c> and is equal to this.
        /// </summary>
        /// <param name="other">The other <see cref="HttpRequestKey"/>.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is not <c>null</c> and is equal to this; <c>false</c> otherwise.</returns>
        public bool Equals(HttpRequestKey? other)
        {
            if (other == null)
                return false;

            return RequestMethod == other.RequestMethod 
                && RequestIdentifier.Equals(other.RequestIdentifier)
                && string.Equals(RequestUri, other.RequestUri, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if object instance is <see cref="HttpRequestKey"/> and equal to this.
        /// </summary>
        /// <param name="obj">The object instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is <see cref="HttpRequestKey"/> and equal to this; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as HttpRequestKey);
        }

        /// <summary>
        /// Gets hash code.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(RequestMethod, RequestIdentifier, RequestUri);
        }
    }
}
