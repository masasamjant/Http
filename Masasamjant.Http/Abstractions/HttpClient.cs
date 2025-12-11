using Masasamjant.Http.Caching;
using Masasamjant.Http.Interceptors;
using Masasamjant.Http.Listeners;
using System.Net.Http.Headers;

namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents abstract HTTP client.
    /// </summary>
    public abstract class HttpClient : IHttpClient
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpClient"/> class.
        /// </summary>
        /// <param name="cacheManager">The <see cref="IHttpCacheManager"/>.</param>
        protected HttpClient(IHttpCacheManager cacheManager)
        {
            CacheManager = cacheManager;
        }

        /// <summary>
        /// Initializes new default instance of the <see cref="HttpClient"/> class.
        /// </summary>
        protected HttpClient()
            : this(HttpCacheManager.Default)
        { }

        /// <summary>
        /// Gets the <see cref="HttpGetRequestInterceptorCollection"/> for interceptors executed 
        /// when HTTP GET request is performed.
        /// </summary>
        public HttpGetRequestInterceptorCollection HttpGetRequestInterceptors { get; } = new HttpGetRequestInterceptorCollection();

        /// <summary>
        /// Gets the <see cref="HttpPostRequestInterceptorCollection"/> for interceptors executed 
        /// when HTTP POST request is performed.
        /// </summary>
        public HttpPostRequestInterceptorCollection HttpPostRequestInterceptors { get; } = new HttpPostRequestInterceptorCollection();

        /// <summary>
        /// Gets the <see cref="HttpClientListenerCollection"/> for listeners of HTTP request execution.
        /// </summary>
        public HttpClientListenerCollection HttpClientListeners { get; } = new HttpClientListenerCollection();

        /// <summary>
        /// Gets the <see cref="IHttpCacheManager"/>.
        /// </summary>
        protected IHttpCacheManager CacheManager { get; }

        /// <summary>
        /// Perform HTTP GET request using specified <see cref="HttpGetRequest"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="request">The <see cref="HttpGetRequest"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public abstract Task<T?> GetAsync<T>(HttpGetRequest request);

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the posted data and result.</typeparam>
        /// <param name="request">The <see cref="HttpPostRequest{T}"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public abstract Task<T?> PostAsync<T>(HttpPostRequest<T> request) where T : notnull;

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type of the posted data.</typeparam>
        /// <param name="request">The <see cref="HttpPostRequest{T}"/> to perform.</param>
        /// <returns>A <typeparamref name="TResult"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public abstract Task<TResult?> PostAsync<TResult, T>(HttpPostRequest<T> request) where T : notnull;

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to perform.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public abstract Task PostAsync(HttpPostRequest request);

        /// <summary>
        /// Executed interceptors to specified <see cref="HttpGetRequest"/> and checks after each interceptor, 
        /// if request should be canceled. If one of the interceptors indicate that request should be canceled, 
        /// then remaining interceptors are not executed.
        /// </summary>
        /// <param name="request">The intercepted <see cref="HttpGetRequest"/>.</param>
        /// <returns><c>true</c> if <paramref name="request"/> was canceled by interceptors; <c>false</c> otherwise.</returns>
        protected async Task<bool> IsCanceledByInterceptorsAsync(HttpGetRequest request)
        {
            var interception = await ExecuteInterceptorsAsync(request);

            if (interception.CancelRequest)
            {
                PerformRequestInterceptionCancellation(request, interception);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Executed interceptors to specified <see cref="HttpPostRequest"/> and checks after each interceptor, 
        /// if request should be canceled. If one of the interceptors indicate that request should be canceled, 
        /// then remaining interceptors are not executed.
        /// </summary>
        /// <param name="request">The intercepted <see cref="HttpPostRequest"/>.</param>
        /// <returns><c>true</c> if <paramref name="request"/> was canceled by interceptors; <c>false</c> otherwise.</returns>
        protected async Task<bool> IsCanceledByInterceptorsAsync(HttpPostRequest request)
        {
            var interception = await ExecuteInterceptorsAsync(request);

            if (interception.CancelRequest)
            {
                PerformRequestInterceptionCancellation(request, interception);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Invokes <see cref="IHttpClientListener.OnExecutingAsync(HttpRequest)"/> of each listener in <see cref="HttpClientListeners"/> 
        /// with specified <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequest"/> about to be executed.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        protected async Task OnExecutingHttpClientListenersAsync(HttpRequest request)
        {
            foreach (var listener in HttpClientListeners)
                await listener.OnExecutingAsync(request);
        }

        /// <summary>
        /// Invokes <see cref="IHttpClientListener.OnExecutedAsync(HttpRequest)"/> of each listener in <see cref="HttpClientListeners"/> 
        /// with specified <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The executed <see cref="HttpRequest"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        protected async Task OnExecutedHttpClientListenersAsync(HttpRequest request)
        {
            foreach (var listener in HttpClientListeners)
                await listener.OnExecutedAsync(request);
        }

        /// <summary>
        /// Invokes <see cref="IHttpClientListener.OnErrorAsync(HttpRequest, Exception)"/> of each listener in <see cref="HttpClientListeners"/>
        /// with specified <see cref="HttpRequest"/> and <see cref="Exception"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequest"/> where exception occurred.</param>
        /// <param name="exception">The occurred <see cref="Exception"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        protected async Task OnErrorHttpClientListenersAsync(HttpRequest request, Exception exception)
        {
            foreach (var listener in HttpClientListeners)
                await listener.OnErrorAsync(request, exception);
        }

        /// <summary>
        /// Adds HTTP headers from specified <see cref="HttpRequest"/> to specified <see cref="HttpHeaders"/>.
        /// </summary>
        /// <param name="request">The HTTP request to read headers.</param>
        /// <param name="headers">The <see cref="HttpHeaders"/> to set headers.</param>
        protected static void AddHttpHeaders(HttpRequest request, HttpHeaders headers)
        {
            if (request.Headers.Count == 0)
                return;

            foreach (var header in request.Headers)
            {
                RemoveExistingHeader(headers, header.Name);
                headers.Add(header.Name, header.Value);
            }
        }

        private static void RemoveExistingHeader(HttpHeaders headers, string headerName)
        {
            if (headers.Contains(headerName))
                headers.Remove(headerName);
        }

        /// <summary>
        /// Cache results of specified HTTP Get request is results can be cached.
        /// </summary>
        /// <param name="request">The <see cref="HttpGetRequest"/>.</param>
        /// <param name="response">The <see cref="HttpResponseMessage"/>.</param>
        /// <param name="contentType">The content type.</param>
        protected async Task CacheResultAsync(HttpGetRequest request, HttpResponseMessage response, string contentType)
        {
            if (request.Caching.CanCacheResult)
                return;

            using (var stream = new MemoryStream())
            {
                await CopyContentAsync(stream, response);
                await CacheStreamContentAsync(stream, request, contentType);
            }
        }

        private static async Task CopyContentAsync(MemoryStream stream, HttpResponseMessage response)
        {
            await response.Content.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
        }

        private async Task CacheStreamContentAsync(MemoryStream stream, HttpGetRequest request, string contentType)
        {
            using (var reader = new StreamReader(stream))
            {
                var value = await reader.ReadToEndAsync();
                await CacheManager.AddCacheContentAsync(request, value, contentType, request.Caching.CacheDuration);
            }
        }

        /// <summary>
        /// Tries to get cached result of the previous HTTP Get request.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="request">The current <see cref="HttpGetRequest"/>.</param>
        /// <returns>A tuple of indication is result was found from cache and the result.</returns>
        protected async Task<(bool Cached, T? Result)> TryGetCacheResultAsync<T>(HttpGetRequest request)
        {
            if (request.Caching.CanCacheResult)
                return (false, default);
          
            var cacheContent = await CacheManager.GetCacheContentAsync(request);

            if (cacheContent != null && cacheContent.ContentValue != null)
            {
                var cacheResult = GetCacheResultFromContent<T>(cacheContent, request);
                return (true, cacheResult);
            }

            return (false, default);
        }

        private T? GetCacheResultFromContent<T>(HttpCacheContent cacheContent, HttpGetRequest request)
        {
            var cacheResult = DeserializeCacheContentValue<T>(cacheContent.ContentValue);
            request.Caching.IsCacheResult = true;
            return cacheResult;
        }

        /// <summary>
        /// Derived classes must override to deserialize cache content value.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object.</typeparam>
        /// <param name="contentValue">The cache content value to deserialize.</param>
        /// <returns>A deserialized value.</returns>
        protected abstract T? DeserializeCacheContentValue<T>(string? contentValue);

        private async Task<HttpRequestInterception> ExecuteInterceptorsAsync(HttpGetRequest request)
        {
            // Execute each interceptor and check if some indicated that request should be canceled.
            foreach (var interceptor in HttpGetRequestInterceptors)
            {
                var interception = await interceptor.InterceptAsync(request);

                if (interception.Result == HttpRequestInterceptionResult.Cancel)
                    return interception;
            }

            // No interceptors or none indicated cancellation.
            return HttpRequestInterception.Continue;
        }

        private async Task<HttpRequestInterception> ExecuteInterceptorsAsync(HttpPostRequest request)
        {
            // Execute each interceptor and check if some indicated that request should be canceled.
            foreach (var interceptor in HttpPostRequestInterceptors)
            {
                var interception = await interceptor.InterceptAsync(request);

                if (interception.Result == HttpRequestInterceptionResult.Cancel)
                    return interception;
            }

            // No interceptors or none indicated cancellation.
            return HttpRequestInterception.Continue;
        }

        private static void PerformRequestInterceptionCancellation(HttpRequest request, HttpRequestInterception interception)
        {
            if (!interception.CancelRequest)
                throw new ArgumentException("The interception indicates that request should not be canceled.", nameof(interception));

            request.Cancel();

            if (interception.ThrowCancelException)
                throw string.IsNullOrWhiteSpace(interception.CancelReason)
                    ? new HttpRequestInterceptionException(request)
                    : new HttpRequestInterceptionException(interception.CancelReason, request);
        }

        /// <summary>
        /// Handles the exception occurred in performing HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="exception">The occurred exception.</param>
        /// <param name="message">The exception message.</param>
        /// <returns>A <see cref="HttpRequestException"/>.</returns>
        protected async Task<HttpRequestException> HandleRequestExceptionAsync(HttpRequest request, Exception exception, string message)
        {
            await OnErrorHttpClientListenersAsync(request, exception);

            if (exception is HttpRequestException requestException)
                return requestException;

            return new HttpRequestException(request, message, exception);
        }
    }
}
