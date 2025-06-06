namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents abstract HTTP request.
    /// </summary>
    public abstract class HttpRequest
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI without query.</param>
        /// <param name="requestMethod">The <see cref="HttpRequestMethod"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="requestUri"/> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ArgumentException">If value of <paramref name="requestMethod"/> is not defined.</exception>
        internal HttpRequest(string requestUri, HttpRequestMethod requestMethod)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentNullException(nameof(requestUri), "The request URI cannot be empty or contain only whitespace characters.");

            if (!Enum.IsDefined(requestMethod))
                throw new ArgumentException("The value is not defined.", nameof(requestMethod));

            RequestUri = requestUri.Trim();
            Identifier = Guid.NewGuid();
            Method = requestMethod;
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
        }

        /// <summary>
        /// Notifies when request is canceled.
        /// </summary>
        public event EventHandler<EventArgs>? Canceled;

        /// <summary>
        /// Gets the unique identifier of the request.
        /// </summary>
        public Guid Identifier { get; }

        /// <summary>
        /// Gets the request URI without query.
        /// </summary>
        public string RequestUri { get; }

        /// <summary>
        /// Gets the collection of HTTP headers.
        /// </summary>
        public HttpHeaderCollection Headers { get; } = new HttpHeaderCollection();

        /// <summary>
        /// Gets the HTTP request method.
        /// </summary>
        public HttpRequestMethod Method { get; }

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets the cancellation token source.
        /// </summary>
        protected CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Cancel request.
        /// </summary>
        public void Cancel()
        {
            CancellationTokenSource.Cancel();
            OnCanceled();
        }

        /// <summary>
        /// Gets the full request URI with query, if specified.
        /// </summary>
        /// <returns>A full request URI.</returns>
        public virtual string GetFullRequestUri()
        {
            return RequestUri;
        }

        /// <summary>
        /// Gets <see cref="HttpRequestKey"/> for this request.
        /// </summary>
        /// <returns>A <see cref="HttpRequestKey"/> that identify this request.</returns>
        public HttpRequestKey GetHttpRequestKey()
        {
            return new HttpRequestKey(this);
        }

        /// <summary>
        /// Invoked when request is canceled. Raises <see cref="Canceled"/> event.
        /// </summary>
        protected virtual void OnCanceled() 
        {
            Canceled?.Invoke(this, EventArgs.Empty);    
        }

        /// <summary>
        /// Copies headers of this request to specified <see cref="HttpRequest"/> instance.
        /// </summary>
        /// <param name="request">The request to copy headers to.</param>
        protected void CloneHeadersTo(HttpRequest request)
        {
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Clone());
            }
        }
    }
}
