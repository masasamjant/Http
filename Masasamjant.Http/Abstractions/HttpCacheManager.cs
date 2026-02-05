using Masasamjant.Http.Caching;
using System.Security.Cryptography;
using System.Text;

namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents abstract manager of caching result of HTTP GET request.
    /// </summary>
    public abstract class HttpCacheManager : IHttpCacheManager
    {
        /// <summary>
        /// Gets the default implementation of <see cref="HttpCacheManager"/> that does not cache content.
        /// </summary>
        public static HttpCacheManager Default { get; } = new DefaultHttpCacheManager();

        /// <summary>
        /// Add specified content to cache.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="contentValue">The content value.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="duration">The cache duration.</param>
        public abstract Task AddCacheContentAsync(HttpGetRequest request, string? contentValue, string? contentType, TimeSpan duration);

        /// <summary>
        /// Gets cached content.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="HttpCacheContent"/> or <c>null</c>.</returns>
        public abstract Task<HttpCacheContent?> GetCacheContentAsync(HttpGetRequest request);

        /// <summary>
        /// Removes cached content.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A task.</returns>
        public abstract Task RemoveCacheContentAsync(HttpGetRequest request);

        /// <summary>
        /// Gets cache content key for specified HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A cache content key.</returns>
        protected static string GetContentKey(HttpGetRequest request)
        {
            var buffer = Encoding.Unicode.GetBytes(request.GetHttpRequestKey().RequestUri);
            buffer = SHA1.HashData(buffer);
            return Convert.ToBase64String(buffer);
        }

        private sealed class DefaultHttpCacheManager : HttpCacheManager
        {
            public override Task AddCacheContentAsync(HttpGetRequest request, string? contentValue, string? contentType, TimeSpan duration)
            {
                return Task.CompletedTask;
            }

            public override Task<HttpCacheContent?> GetCacheContentAsync(HttpGetRequest request)
            {
                return Task.FromResult<HttpCacheContent?>(null);
            }

            public override Task RemoveCacheContentAsync(HttpGetRequest request)
            {
                return Task.CompletedTask;
            }
        }
    }
}
