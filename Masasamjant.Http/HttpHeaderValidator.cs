namespace Masasamjant.Http
{
    /// <summary>
    /// Validator to validate HTTP header name and value. 
    /// </summary>
    public static class HttpHeaderValidator
    {
        private const int MaxNameLength = 40;
        private static readonly char[] nameWhitelist = "-_".ToCharArray();
        private static readonly char[] valueWhitelist = "_ :;.,\\/\"'?!(){}[]@<>=-+*#$&`|~^%".ToCharArray();

        /// <summary>
        /// Check if specified name is valid name of HTTP header.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns><c>true</c> if value of <paramref name="name"/> has no invalid characters and is not too long; <c>false</c> otherwise.</returns>
        public static bool IsValidHeaderName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > MaxNameLength)
                return false;

            var invalid = GetFirstInvalidCharacter(name, nameWhitelist);

            if (invalid.HasValue)
                return false;

            return true;
        }

        /// <summary>
        /// Validate that specified name is valid name of HTTP header.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="name"/> is empty or only whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If value of <paramref name="name"/> is more than 40 characters.</exception>
        /// <exception cref="ArgumentException">If value of <paramref name="name"/> contains invalid character that is not valid in name of HTTP header.</exception>
        public static void ValidateHeaderName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "HTTP header name cannot be empty or only whitespace.");

            if (name.Length > MaxNameLength)
                throw new ArgumentOutOfRangeException(nameof(name), name.Length, $"HTTP header name cannot be more than {MaxNameLength} characters.");

            var invalid = GetFirstInvalidCharacter(name, nameWhitelist);

            if (invalid.HasValue)
                throw new ArgumentException($"{invalid.Value} is not valid character in HTTP header name.", nameof(name));
        }

        /// <summary>
        /// Check if specified value is valid HTTP header value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if value of <paramref name="value"/> has no invalid characters; <c>false</c> otherwise.</returns>
        public static bool IsValidHeaderValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            var invalid = GetFirstInvalidCharacter(value, valueWhitelist);

            if (invalid.HasValue)
                return false;

            return true;
        }

        /// <summary>
        /// Validate that specified value is valid HTTP header value. 
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <exception cref="ArgumentException">If value of <paramref name="value"/> contains invalid character that is not valid in value of HTTP header.</exception>
        public static void ValidateHeaderValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var invalid = GetFirstInvalidCharacter(value, valueWhitelist);

            if (invalid.HasValue)
                throw new ArgumentException($"{invalid.Value} is not valid character in HTTP header value.", nameof(value));
        }

        private static char? GetFirstInvalidCharacter(string value, char[] whitelist)
        {
            foreach (char c in value)
            {
                if (char.IsAsciiDigit(c) || char.IsAsciiLetter(c) || whitelist.Contains(c))
                    continue;
                else
                    return c;
            }

            return null;
        }
    }
}
