namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents component to build instance of <see cref="IHttpClient"/> implementation.
    /// </summary>
    public interface IHttpClientBuilder
    {
        /// <summary>
        /// Builds instance of <see cref="IHttpClient"/> implementation for specified purpose.
        /// </summary>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        IHttpClient Build(string clientPurpose);
    }
}
