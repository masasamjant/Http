namespace Masasamjant.Http
{
    /// <summary>
    /// Represents attribute applied to property to indicate that property value can be used as HTTP parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class HttpParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpParameterAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="parameterName"/> is empty or contains only whitespace characters.</exception>
        public HttpParameterAttribute(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName), "The parameter name cannot be empty or contain only whitespace characters.");

            ParameterName = parameterName.Trim();
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string ParameterName { get; }
    }
}
