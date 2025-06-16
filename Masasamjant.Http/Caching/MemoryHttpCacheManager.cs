using Masasamjant.Http.Abstractions;

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
                if (keys.ContainsKey(contentKey))
                    keys.Remove(contentKey);

                var uri = request.RequestUri;

                if (!contentsLookup.TryGetValue(uri, out var contents))
                {
                    contents = new Dictionary<string, HttpCacheContent>();
                    contentsLookup.Add(uri, contents);
                }

                if (contents.ContainsKey(contentKey))
                    contents.Remove(contentKey);

                DateTimeOffset expires = DateTimeOffset.UtcNow.Add(duration);
                keys.Add(contentKey, expires);
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
                {
                    if (contentsLookup.TryGetValue(uri, out var contents))
                    {
                        if (contents.TryGetValue(contentKey, out content))
                        {
                            bool expired = expires < DateTimeOffset.UtcNow;

                            if (expired)
                            {
                                keys.Remove(contentKey);
                                contents.Remove(contentKey);
                                content = null;
                            }
                        }
                    }
                }
                else
                {
                    if (contentsLookup.TryGetValue(uri, out var contents))
                        contents.Remove(contentKey);
                }
            }

            return Task.FromResult(content);
        }
    }
}
