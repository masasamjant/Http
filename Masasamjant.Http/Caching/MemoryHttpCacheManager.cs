using Masasamjant.Http.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Masasamjant.Http.Caching
{
    /// <summary>
    /// Represents manager of caching result of HTTP GET request to memory.
    /// </summary>
    public sealed class MemoryHttpCacheManager : HttpCacheManager
    {
        private readonly Dictionary<string, Dictionary<string, HttpCacheContent>> contentsLookup;
        private readonly Dictionary<string, DateTimeOffset> keys;
        private readonly Lock cacheLock;

        /// <summary>
        /// Initializes new instance of the <see cref="MemoryHttpCacheManager"/> class.
        /// </summary>
        public MemoryHttpCacheManager()
        {
            cacheLock = new Lock();
            contentsLookup = new Dictionary<string, Dictionary<string, HttpCacheContent>>();
            keys = new Dictionary<string, DateTimeOffset>();
        }

        /// <summary>
        /// Add specified content to cache.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="contentValue">The content value.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="duration">The cache duration.</param>
        public override Task AddCacheContentAsync(HttpGetRequest request, string? contentValue, string? contentType, TimeSpan duration)
        {
            var contentKey = GetContentKey(request);
        
            lock (cacheLock)
            {
                keys.Remove(contentKey);
                var contents = GetOrAddCacheContent(request.RequestUri);
                contents.Remove(contentKey);
                AddContentKey(contentKey, duration);
                contents.Add(contentKey, new HttpCacheContent(contentKey, contentValue, contentType));
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets cached content.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="HttpCacheContent"/> or <c>null</c>.</returns>
        public override Task<HttpCacheContent?> GetCacheContentAsync(HttpGetRequest request)
        {
            var contentKey = GetContentKey(request);

            HttpCacheContent? content = null;

            lock (cacheLock)
            { 
                var uri = request.RequestUri;

                if (keys.TryGetValue(contentKey, out var expires))
                    content = GetHttpCacheContent(uri, contentKey, expires);
            }

            return Task.FromResult(content);
        }

        /// <summary>
        /// Removes cached content.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A task.</returns>
        public override Task RemoveCacheContentAsync(HttpGetRequest request)
        {
            var contentKey = GetContentKey(request);
            var uri = request.RequestUri;

            lock (cacheLock)
            {
                if (TryGetCacheContent(uri, contentKey, out var contents, out var content))
                { 
                    RemoveCachedContent(contents, contentKey);
                }
            }

            return Task.CompletedTask;
        }

        private Dictionary<string, HttpCacheContent> GetOrAddCacheContent(string uri)
        {
            if (!contentsLookup.TryGetValue(uri, out var contents))
            {
                contents = new Dictionary<string, HttpCacheContent>();
                contentsLookup.Add(uri, contents);
            }

            return contents;
        }

        private void AddContentKey(string contentKey, TimeSpan duration)
        {
            DateTimeOffset expires = DateTimeOffset.UtcNow.Add(duration);
            keys.Add(contentKey, expires);
        }

        private HttpCacheContent? GetHttpCacheContent(string uri, string contentKey, DateTimeOffset expires)
        {
            if (TryGetCacheContent(uri, contentKey, out var contents, out var content))
            {
                if (IsExpired(expires))
                {
                    RemoveCachedContent(contents, contentKey);
                    return null;
                }
                else
                    return content;
            }

            return null;
        }

        private bool TryGetCacheContent(string uri, string contentKey, [NotNullWhen(true)] out Dictionary<string, HttpCacheContent>? contents, [NotNullWhen(true)] out HttpCacheContent? content)
        {
            if (contentsLookup.TryGetValue(uri, out contents) &&
                contents.TryGetValue(contentKey, out content))
                return true;

            contents = null;
            content = null;
            return false;
        }

        private static bool IsExpired(DateTimeOffset expires)
            => expires < DateTimeOffset.UtcNow;

        private void RemoveCachedContent(Dictionary<string, HttpCacheContent> contents, string contentKey)
        {
            keys.Remove(contentKey);
            contents.Remove(contentKey);
            contentsLookup.Remove(contentKey);
        }
    }
}
