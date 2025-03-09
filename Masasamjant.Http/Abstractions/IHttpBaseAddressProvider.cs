namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents provider of HTTP base address.
    /// </summary>
    public interface IHttpBaseAddressProvider
    {
        /// <summary>
        /// Gets the HTTP base address.
        /// </summary>
        /// <returns>A HTTP base address.</returns>
        /// <exception cref="InvalidOperationException">If could not get HTTP base address.</exception>
        string GetHttpBaseAdress();
    }
}
