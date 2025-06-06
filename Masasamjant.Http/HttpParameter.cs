namespace Masasamjant.Http
{
    /// <summary>
    /// Represents parameter of HTTP Get request.
    /// </summary>
    public sealed class HttpParameter : IEquatable<HttpParameter>, ICloneable
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpParameter"/> class.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is empty or contains only whitespace characters.</exception>
        public HttpParameter(string name, object? value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "The name cannot be empty or contain only whitespace characters.");

            Name = name.Trim();
            Value = value;
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Creates <see cref="HttpParameter"/> instance.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="HttpParameter"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is empty or contains only whitespace characters.</exception>
        public static HttpParameter From(string name, object? value = null) => new HttpParameter(name, value);

        /// <summary>
        /// Check if other <see cref="HttpParameter"/> has same name with this parameter.
        /// </summary>
        /// <param name="other">The other <see cref="HttpParameter"/>.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is not <c>null</c> and has same name with this; <c>false</c> otherwise.</returns>
        public bool Equals(HttpParameter? other)
        {
            return other != null && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if object instance is <see cref="HttpParameter"/> and has same name with this parameter.
        /// </summary>
        /// <param name="obj">The object instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is <see cref="HttpParameter"/> and has same name with this; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as HttpParameter);
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
        /// Gets string presentation in "Name=Value" format.
        /// </summary>
        /// <returns>A string presentation.</returns>
        public override string ToString()
        {
            if (Value == null)
                return $"{Name}=";

            return $"{Name}={Value}";
        }

        /// <summary>
        /// Creates a copy from this parameter.
        /// </summary>
        /// <returns>A copy from this parameter.</returns>
        public HttpParameter Clone()
        {
            return new HttpParameter(Name, Value);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
