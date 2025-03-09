namespace Masasamjant.Http.Caching
{
    /// <summary>
    /// Represents options for caching result of HTTP Get request.
    /// </summary>
    public sealed class HttpGetRequestCaching
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpGetRequestCaching"/> class.
        /// </summary>
        /// <param name="canCacheResult"><c>true</c> if result can be cached; <c>false</c> otherwise.</param>
        /// <param name="cacheDuration">The duration for how long result can be cached. If 0 or less, then results are not cached.</param>
        public HttpGetRequestCaching(bool canCacheResult, TimeSpan cacheDuration)
        {
            CanCacheResult = canCacheResult && cacheDuration > TimeSpan.Zero;
            CacheDuration = cacheDuration;
            IsCacheResult = false;
        }

        /// <summary>
        /// Initializes new instance of the <see cref="HttpGetRequestCaching"/> class that does not allow caching.
        /// </summary>
        public HttpGetRequestCaching()
        {
            CanCacheResult = false;
            CacheDuration = TimeSpan.Zero;
            IsCacheResult = false;
        }

        /// <summary>
        /// Gets whether or not result can be cached.
        /// </summary>
        public bool CanCacheResult { get; }

        /// <summary>
        /// Gets the duration for how long result can be cached.
        /// </summary>
        public TimeSpan CacheDuration { get; }

        /// <summary>
        /// Gets whether or not result was read from cache.
        /// </summary>
        public bool IsCacheResult { get; internal set; }
    }
}
