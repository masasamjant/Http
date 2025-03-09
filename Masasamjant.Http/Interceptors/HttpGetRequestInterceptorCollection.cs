using Masasamjant.Http.Abstractions;
using System.Collections;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents collection of HTTP GET request interceptors.
    /// </summary>
    public sealed class HttpGetRequestInterceptorCollection : ICollection<IHttpGetRequestInterceptor>, IEnumerable<IHttpGetRequestInterceptor>
    {
        private readonly List<IHttpGetRequestInterceptor> interceptors;
    
        /// <summary>
        /// Initializes new instance of the <see cref="HttpGetRequestInterceptorCollection"/> class.
        /// </summary>
        public HttpGetRequestInterceptorCollection()
        {
            interceptors = new List<IHttpGetRequestInterceptor>();
        }

        /// <summary>
        /// Gets the count of interceptors in collection.
        /// </summary>
        public int Count => interceptors.Count;

        /// <summary>
        /// Adds specified <see cref="IHttpGetRequestInterceptor"/> to collection,
        /// if not already part of the collection.
        /// </summary>
        /// <param name="interceptor">The <see cref="IHttpGetRequestInterceptor"/> to add.</param>
        public void Add(IHttpGetRequestInterceptor interceptor)
        {
            if (!Contains(interceptor))
                interceptors.Add(interceptor);
        }

        /// <summary>
        /// Clears collection by removing all interceptors.
        /// </summary>
        public void Clear() => interceptors.Clear();

        /// <summary>
        /// Check if collection contains specified <see cref="IHttpGetRequestInterceptor"/>.
        /// </summary>
        /// <param name="interceptor">The <see cref="IHttpGetRequestInterceptor"/>.</param>
        /// <returns><c>true</c> if collection contains interceptor; <c>false</c> otherwise.</returns>
        public bool Contains(IHttpGetRequestInterceptor interceptor) => interceptors.Contains(interceptor);

        /// <summary>
        /// Gets enumerator to iterate all interceptors in collection.
        /// </summary>
        /// <returns>A enumerator to iterate interceptors.</returns>
        public IEnumerator<IHttpGetRequestInterceptor> GetEnumerator()
        {
            foreach (var interceptor in interceptors)
                yield return interceptor;
        }

        /// <summary>
        /// Remove specified <see cref="IHttpGetRequestInterceptor"/> from collection.
        /// </summary>
        /// <param name="interceptor">The <see cref="IHttpGetRequestInterceptor"/> to remove.</param>
        /// <returns><c>true</c> if interceptor was removed; <c>false</c> if interceptor was not in collection.</returns>
        public bool Remove(IHttpGetRequestInterceptor interceptor) => interceptors.Remove(interceptor);

        bool ICollection<IHttpGetRequestInterceptor>.IsReadOnly => false;

        void ICollection<IHttpGetRequestInterceptor>.CopyTo(IHttpGetRequestInterceptor[] array, int arrayIndex)
        {
            interceptors.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
