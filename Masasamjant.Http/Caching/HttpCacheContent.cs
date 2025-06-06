using System.Text.Json.Serialization;

namespace Masasamjant.Http.Caching
{
    /// <summary>
    /// Represents content of HTTP cache.
    /// </summary>
    public class HttpCacheContent
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpCacheContent"/> class.
        /// </summary>
        /// <param name="contentKey">The content key.</param>
        /// <param name="contentValue">The content value.</param>
        /// <param name="contentType">The content type.</param>
        public HttpCacheContent(string contentKey, string? contentValue, string? contentType)
        {
            ContentKey = contentKey;
            ContentValue = contentValue;
            ContentType = contentType;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="HttpCacheContent"/> class.
        /// </summary>
        public HttpCacheContent() 
        { }

        /// <summary>
        /// Gets the content key.
        /// </summary>
        [JsonInclude]
        public string ContentKey { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the content value.
        /// </summary>
        [JsonInclude]
        public string? ContentValue { get; internal set; }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        [JsonInclude]
        public string? ContentType { get; internal set; }
    }
}
