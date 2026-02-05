using Masasamjant.Http.Caching;

namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents manager of caching result of HTTP GET request.
    /// </summary>
    public interface IHttpCacheManager
    {
        /// <summary>
        /// Add specified content to cache.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="contentValue">The content value.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="duration">The cache duration.</param>
        Task AddCacheContentAsync(HttpGetRequest request, string? contentValue, string? contentType, TimeSpan duration);

        /// <summary>
        /// Gets cached content.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="HttpCacheContent"/> or <c>null</c>.</returns>
        Task<HttpCacheContent?> GetCacheContentAsync(HttpGetRequest request);

        /// <summary>
        /// Removes cached content.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A task.</returns>
        Task RemoveCacheContentAsync(HttpGetRequest request);
    }
}
