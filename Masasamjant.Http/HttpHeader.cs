namespace Masasamjant.Http
{
    /// <summary>
    /// Represents header send with HTTP request.
    /// </summary>
    public sealed class HttpHeader : IEquatable<HttpHeader>
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpHeader"/> class.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is empty or contains only white-space characters.</exception>
        public HttpHeader(string name, string? value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "The name cannot be empty or contain only white-space characters.");

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
    }
}
