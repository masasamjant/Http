using System.Collections;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents collection of HTTP headers.
    /// </summary>
    public sealed class HttpHeaderCollection : ICollection<HttpHeader>, IEnumerable<HttpHeader>
    {
        private readonly HashSet<HttpHeader> headers;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpHeaderCollection"/> class.
        /// </summary>
        public HttpHeaderCollection() 
        {
            headers = new HashSet<HttpHeader>();
        }

        /// <summary>
        /// Gets the count of headers.
        /// </summary>
        public int Count => headers.Count;

        /// <summary>
        /// Add specified <see cref="HttpHeader"/> to collection.
        /// </summary>
        /// <param name="header">The <see cref="HttpHeader"/> to add.</param>
        /// <exception cref="ArgumentException">If collection already contains header with same name.</exception>
        public void Add(HttpHeader header)
        {
            if (!headers.Add(header))
                throw new ArgumentException($"The collection already contains '{header.Name}' HTTP header.", nameof(header));
        }

        /// <summary>
        /// Add <see cref="HttpHeader"/> with specified name and value.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <returns>A added <see cref="HttpHeader"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is empty or contains only white-space characters.</exception>
        /// <exception cref="ArgumentException">If collection already contains header with same name.</exception>
        public HttpHeader Add(string name, string? value)
        {
            var header = new HttpHeader(name, value);
            Add(header);
            return header;
        }

        /// <summary>
        /// Clear collection.
        /// </summary>
        public void Clear() => headers.Clear();

        /// <summary>
        /// Check if collection contains specified <see cref="HttpHeader"/>.
        /// </summary>
        /// <param name="header">The <see cref="HttpHeader"/>.</param>
        /// <returns><c>true</c> if collection contains <paramref name="header"/>; <c>false</c> otherwise.</returns>
        public bool Contains(HttpHeader header) => headers.Contains(header);

        /// <summary>
        /// Check if collection contains <see cref="HttpHeader"/> with specified name.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <returns><c>true</c> if collection contains <see cref="HttpHeader"/> with specified name; <c>false</c> otherwise.</returns>
        public bool Contains(string name) => headers.Any(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Gets enumerator to iterate all headers in collection.
        /// </summary>
        /// <returns>A enumerator to iterate headers.</returns>
        public IEnumerator<HttpHeader> GetEnumerator()
        {
            foreach (var header in headers)
                yield return header;
        }

        /// <summary>
        /// Gets the <see cref="HttpHeader"/> with specified name.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <returns>A <see cref="HttpHeader"/> or <c>null</c>, if not exist.</returns>
        public HttpHeader? Get(string name)
        {
            return headers.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Removes specified <see cref="HttpHeader"/> from collection.
        /// </summary>
        /// <param name="header">The <see cref="HttpHeader"/> to remove.</param>
        /// <returns><c>true</c> <paramref name="header"/> was removed; <c>false</c> if not in collection.</returns>
        public bool Remove(HttpHeader header) => headers.Remove(header);

        /// <summary>
        /// Removes <see cref="HttpHeader"/> specified by name from collection.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <returns><c>true</c> if collection contained header with specified name; <c>false</c> otherwise.</returns>
        public bool Remove(string name)
        {
            var header = headers.FirstOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));

            if (header == null)
                return false;

            return headers.Remove(header);
        }

        bool ICollection<HttpHeader>.IsReadOnly => false;

        void ICollection<HttpHeader>.CopyTo(HttpHeader[] array, int arrayIndex)
        {
            headers.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator() 
        {
            return GetEnumerator();
        }
    }
}
