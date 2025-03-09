using Masasamjant.Http.Abstractions;
using System.Collections;

namespace Masasamjant.Http.Interceptors
{
    /// <summary>
    /// Represents collection of HTTP POST request interceptors.
    /// </summary>
    public sealed class HttpPostRequestInterceptorCollection : ICollection<IHttpPostRequestInterceptor>, IEnumerable<IHttpPostRequestInterceptor>
    {
        private readonly List<IHttpPostRequestInterceptor> interceptors;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpPostRequestInterceptorCollection"/> class.
        /// </summary>
        public HttpPostRequestInterceptorCollection()
        {
            interceptors = new List<IHttpPostRequestInterceptor>();
        }

        /// <summary>
        /// Gets the count of interceptors in collection.
        /// </summary>
        public int Count => interceptors.Count;

        /// <summary>
        /// Adds specified <see cref="IHttpPostRequestInterceptor"/> to collection,
        /// if not already part of the collection.
        /// </summary>
        /// <param name="interceptor">The <see cref="IHttpPostRequestInterceptor"/> to add.</param>
        public void Add(IHttpPostRequestInterceptor interceptor)
        {
            if (!Contains(interceptor))
                interceptors.Add(interceptor);
        }

        /// <summary>
        /// Clears collection by removing all interceptors.
        /// </summary>
        public void Clear() => interceptors.Clear();

        /// <summary>
        /// Check if collection contains specified <see cref="IHttpPostRequestInterceptor"/>.
        /// </summary>
        /// <param name="interceptor">The <see cref="IHttpPostRequestInterceptor"/>.</param>
        /// <returns><c>true</c> if collection contains interceptor; <c>false</c> otherwise.</returns>
        public bool Contains(IHttpPostRequestInterceptor interceptor) => interceptors.Contains(interceptor);

        /// <summary>
        /// Gets enumerator to iterate all interceptors in collection.
        /// </summary>
        /// <returns>A enumerator to iterate interceptors.</returns>
        public IEnumerator<IHttpPostRequestInterceptor> GetEnumerator()
        {
            foreach (var interceptor in interceptors)
                yield return interceptor;
        }

        /// <summary>
        /// Remove specified <see cref="IHttpPostRequestInterceptor"/> from collection.
        /// </summary>
        /// <param name="interceptor">The <see cref="IHttpPostRequestInterceptor"/> to remove.</param>
        /// <returns><c>true</c> if interceptor was removed; <c>false</c> if interceptor was not in collection.</returns>
        public bool Remove(IHttpPostRequestInterceptor interceptor) => interceptors.Remove(interceptor);

        bool ICollection<IHttpPostRequestInterceptor>.IsReadOnly => false;

        void ICollection<IHttpPostRequestInterceptor>.CopyTo(IHttpPostRequestInterceptor[] array, int arrayIndex)
        {
            interceptors.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
