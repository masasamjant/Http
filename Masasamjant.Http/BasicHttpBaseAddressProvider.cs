using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents basic <see cref="IHttpBaseAddressProvider"/> that provides specified value.
    /// </summary>
    public sealed class BasicHttpBaseAddressProvider : IHttpBaseAddressProvider
    {
        private readonly string httpBaseAddress;

        /// <summary>
        /// Initializes new instance of the <see cref="BasicHttpBaseAddressProvider"/> class.
        /// </summary>
        /// <param name="httpBaseAddress">The HTTP base address value.</param>
        /// <exception cref="ArgumentNullException">If value of <paramref name="httpBaseAddress"/> is empty or only whitespace.</exception>
        public BasicHttpBaseAddressProvider(string httpBaseAddress)
        {
            if (string.IsNullOrWhiteSpace(httpBaseAddress))
                throw new ArgumentNullException(nameof(httpBaseAddress), "The HTTP base address cannot be empty or only whitespace.");

            this.httpBaseAddress = httpBaseAddress;
        }

        /// <summary>
        /// Gets the HTTP base address.
        /// </summary>
        /// <returns>A HTTP base address.</returns>
        /// <exception cref="InvalidOperationException">If could not get HTTP base address.</exception>
        public string GetHttpBaseAdress()
        {
            return this.httpBaseAddress;
        }
    }
}
