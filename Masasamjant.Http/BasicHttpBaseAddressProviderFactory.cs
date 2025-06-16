using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents <see cref="IHttpBaseAddressProviderFactory"/> that create <see cref="BasicHttpBaseAddressProvider"/> instances.
    /// </summary>
    public sealed class BasicHttpBaseAddressProviderFactory : IHttpBaseAddressProviderFactory
    {
        private readonly Dictionary<string, string> httpBaseAddresses;

        /// <summary>
        /// Initializes new instance of the <see cref="BasicHttpBaseAddressProviderFactory"/> class.
        /// </summary>
        /// <param name="httpBaseAddresses">The HTTP base address purpose and value pairs.</param>
        /// <exception cref="ArgumentException">If <paramref name="httpBaseAddresses"/> is empty.</exception>
        public BasicHttpBaseAddressProviderFactory(Dictionary<string, string> httpBaseAddresses)
        {
            if (httpBaseAddresses.Count == 0)
                throw new ArgumentException("The HTTP base addresses lookup is empty.", nameof(httpBaseAddresses));

            this.httpBaseAddresses = httpBaseAddresses;
        }

        /// <summary>
        /// Creates instance of <see cref="BasicHttpBaseAddressProvider"/> implementation for specified base address purpose.
        /// </summary>
        /// <param name="baseAddressPurpose">The purpose of the HTTP base address.</param>
        /// <returns>A <see cref="BasicHttpBaseAddressProvider"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="baseAddressPurpose"/> is empty or contains only whitespace characters.</exception>
        /// <exception cref="InvalidOperationException">If failed to create instance of <see cref="BasicHttpBaseAddressProvider"/> implementation.</exception>
        public IHttpBaseAddressProvider GetBaseAddressProvider(string baseAddressPurpose)
        {
            if (string.IsNullOrWhiteSpace(baseAddressPurpose))
                throw new ArgumentNullException(nameof(baseAddressPurpose), "The HTTP base address purpose cannot be empty or only whitespace.");

            if (!httpBaseAddresses.TryGetValue(baseAddressPurpose, out var httpBaseAddress))
                throw new ArgumentException($"No HTTP base address for '{baseAddressPurpose}' purpose.", nameof(baseAddressPurpose));

            if (string.IsNullOrWhiteSpace(httpBaseAddress))
                throw new ArgumentException($"HTTP base address for '{baseAddressPurpose}' has empty or only whitespace value.", nameof(baseAddressPurpose));

            return new BasicHttpBaseAddressProvider(httpBaseAddress);
        }
    }
}
