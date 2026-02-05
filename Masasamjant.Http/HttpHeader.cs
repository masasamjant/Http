namespace Masasamjant.Http
{
    /// <summary>
    /// Represents header send with HTTP request.
    /// </summary>
    public sealed class HttpHeader : IEquatable<HttpHeader>, ICloneable
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpHeader"/> class.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="name"/> is empty or only whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If value of <paramref name="name"/> is too long.</exception>
        /// <exception cref="ArgumentException">
        /// If value of <paramref name="name"/> contains character that is invalid in HTTP header name.
        /// -or-
        /// If value of <paramref name="value"/> contains character that is invalid in HTTP header value.
        /// </exception>
        public HttpHeader(string name, string? value)
        {
            HttpHeaderValidator.ValidateHeaderName(name);
            HttpHeaderValidator.ValidateHeaderValue(value);
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the header name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the header value.
        /// </summary>
        public string? Value { get; }

        /// <summary>
        /// Check if other <see cref="HttpHeader"/> has same name with this header.
        /// </summary>
        /// <param name="other">The other <see cref="HttpHeader"/>.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is not <c>null</c> and has same name with this; <c>false</c> otherwise.</returns>
        public bool Equals(HttpHeader? other) 
        {
            return other != null && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if object instance is <see cref="HttpHeader"/> and has same name with this header.
        /// </summary>
        /// <param name="obj">The object instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is <see cref="HttpHeader"/> and has same name with this; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as HttpHeader);
        }

        /// <summary>
        /// Gets hash code.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode() 
        {
            return Name.ToLowerInvariant().GetHashCode();
        }

        /// <summary>
        /// Creates a copy from this header.
        /// </summary>
        /// <returns>A copy from this header.</returns>
        public HttpHeader Clone()
        {
            return new HttpHeader(Name, Value);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
