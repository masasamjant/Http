using Masasamjant.Http.Abstractions;
using System.Collections;

namespace Masasamjant.Http.Listeners
{
    /// <summary>
    /// Represents collection of HTTP listeners.
    /// </summary>
    public sealed class HttpClientListenerCollection : ICollection<IHttpClientListener>, IEnumerable<IHttpClientListener>
    {
        private readonly List<IHttpClientListener> listeners;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpClientListenerCollection"/> class.
        /// </summary>
        public HttpClientListenerCollection()
        {
            listeners = new List<IHttpClientListener>();
        }

        /// <summary>
        /// Gets count of listeners in collection.
        /// </summary>
        public int Count => listeners.Count;

        /// <summary>
        /// Gets enumerator to iterate all listeners in collection.
        /// </summary>
        /// <returns>A enumerator to iterate listeners.</returns>
        public IEnumerator<IHttpClientListener> GetEnumerator()
        {
            foreach (var listener in listeners)
                yield return listener;
        }

        /// <summary>
        /// Clears collection by removing all listeners.
        /// </summary>
        public void Clear() => listeners.Clear();

        /// <summary>
        /// Adds specified <see cref="IHttpClientListener"/> to collection, 
        /// if not already added.
        /// </summary>
        /// <param name="listener">The <see cref="IHttpClientListener"/> to add.</param>
        public void Add(IHttpClientListener listener)
        {
            if (!Contains(listener))
                listeners.Add(listener);
        }

        /// <summary>
        /// Check if collection contains specified <see cref="IHttpClientListener"/>.
        /// </summary>
        /// <param name="listener">The <see cref="IHttpClientListener"/>.</param>
        /// <returns><c>true</c> if collection contains listener; <c>false</c> otherwise.</returns>
        public bool Contains(IHttpClientListener listener) => listeners.Contains(listener);

        /// <summary>
        /// Remove specified <see cref="IHttpClientListener"/> from collection.
        /// </summary>
        /// <param name="listener">The <see cref="IHttpClientListener"/> to remove.</param>
        /// <returns><c>true</c> if listener was removed; <c>false</c> if listener was not in collection.</returns>
        public bool Remove(IHttpClientListener listener) => listeners.Remove(listener);

        bool ICollection<IHttpClientListener>.IsReadOnly => false;

        void ICollection<IHttpClientListener>.CopyTo(IHttpClientListener[] array, int arrayIndex)
        {
            listeners.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
