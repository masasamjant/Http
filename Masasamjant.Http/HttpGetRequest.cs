using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Caching;
using System.Text;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents HTTP Get request.
    /// </summary>
    public sealed class HttpGetRequest : HttpRequest
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpGetRequest"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI without query.</param>
        /// <param name="caching">The <see cref="HttpGetRequestCaching"/> or <c>null</c>, if not cached.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="requestUri"/> is empty or contains only whitespace characters.</exception>
        public HttpGetRequest(string requestUri, HttpGetRequestCaching? caching = null)
            : base(requestUri, HttpRequestMethod.Get)
        {
            Caching = caching ?? new HttpGetRequestCaching();
        }

        /// <summary>
        /// Initializes new instance of the <see cref="HttpGetRequest"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI without query.</param>
        /// <param name="parameters">The HTTP parameters.</param>
        /// <param name="caching">The <see cref="HttpGetRequestCaching"/> or <c>null</c>, if not cached.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="requestUri"/> is empty or contains only whitespace characters.</exception>
        public HttpGetRequest(string requestUri, IEnumerable<HttpParameter> parameters, HttpGetRequestCaching? caching = null)
            : this(requestUri, caching)
        { 
            foreach (var parameter in parameters)
                Parameters.Add(parameter);
        }

        /// <summary>
        /// Gets the <see cref="HttpGetRequestCaching"/> of this request.
        /// </summary>
        public HttpGetRequestCaching Caching { get; }

        /// <summary>
        /// Gets the <see cref="HttpParameterCollection"/> of parameters of this request.
        /// </summary>
        public HttpParameterCollection Parameters { get; } = new HttpParameterCollection();

        /// <summary>
        /// Gets the query string.
        /// </summary>
        public string QueryString => CreateQueryString();

        /// <summary>
        /// Gets the full request URI with query.
        /// </summary>
        public string FullRequestUri
        {
            get
            {
                var qs = QueryString;
                return string.IsNullOrWhiteSpace(qs) ? RequestUri : RequestUri + qs;
            }
        }

        /// <summary>
        /// Gets the full request URI with query, if specified.
        /// </summary>
        /// <returns>A full request URI.</returns>
        public override string GetFullRequestUri()
        {
            return FullRequestUri;
        }

        private string CreateQueryString()
        {
            var parameters = Parameters.ToList();

            if (parameters.Count == 0)
                return string.Empty;

            var builder = new StringBuilder("?");

            foreach (var parameter in parameters)
            {
                builder.Append(parameter.ToString());
                builder.Append('&');
            }

            return builder.ToString().TrimEnd('&');
        }
    }
}
