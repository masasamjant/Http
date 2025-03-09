namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents factory to create instances of <see cref="IHttpBaseAddressProvider"/> implementation for different purposes.
    /// </summary>
    public interface IHttpBaseAddressProviderFactory
    {
        /// <summary>
        /// Creates instance of <see cref="IHttpBaseAddressProvider"/> implementation for specified base address purpose.
        /// </summary>
        /// <param name="baseAddressPurpose">The purpose of the HTTP base address.</param>
        /// <returns>A <see cref="IHttpBaseAddressProvider"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="baseAddressPurpose"/> is empty or contains only whitespace characters.</exception>
        /// <exception cref="InvalidOperationException">If failed to create instance of <see cref="IHttpBaseAddressProvider"/> implementation.</exception>
        IHttpBaseAddressProvider GetBaseAddressProvider(string baseAddressPurpose);
    }
}
