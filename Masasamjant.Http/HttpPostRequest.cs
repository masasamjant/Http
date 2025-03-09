using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents HTTP Post request.
    /// </summary>
    public class HttpPostRequest : HttpRequest
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpPostRequest"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI without query.</param>
        /// <param name="data">The posted data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="requestUri"/> is empty or contains only whitespace characters.</exception>
        public HttpPostRequest(string requestUri, object data)
            : base(requestUri, HttpRequestMethod.Post)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the posted data.
        /// </summary>
        public object Data { get; }
    }

    /// <summary>
    /// Represents HTTP Post request.
    /// </summary>
    /// <typeparam name="T">The type of the post data.</typeparam>
    public sealed class HttpPostRequest<T> : HttpPostRequest where T : notnull 
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpPostRequest"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI without query.</param>
        /// <param name="data">The posted data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="requestUri"/> is empty or contains only whitespace characters.</exception>
        public HttpPostRequest(string requestUri, T data)
            : base(requestUri, data)
        { }

        /// <summary>
        /// Gets the posted data.
        /// </summary>
        public new T Data
        {
            get { return (T)base.Data; }    
        }
    }
}
