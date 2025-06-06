using HttpClient = Masasamjant.Http.Abstractions.HttpClient;
using Masasamjant.Http.Abstractions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Masasamjant.Http.Json
{
    /// <summary>
    /// Represents HTTP client that accepts JSON data.
    /// </summary>
    public sealed class JsonHttpClient : HttpClient
    {
        private readonly System.Net.Http.HttpClient httpClient;
        private const string ContentType = "application/json";

        /// <summary>
        /// Initializes new instance of the <see cref="JsonHttpClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="httpBaseAddressProvider">The <see cref="IHttpBaseAddressProvider"/>.</param>
        public JsonHttpClient(IHttpClientFactory httpClientFactory, IHttpBaseAddressProvider httpBaseAddressProvider, IHttpCacheManager? cacheManager)
        {
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(httpBaseAddressProvider.GetHttpBaseAdress());
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
            CacheManager = cacheManager ?? HttpCacheManager.Default;
        }

        private IHttpCacheManager CacheManager { get; }

        /// <summary>
        /// Perform HTTP GET request using specified <see cref="HttpGetRequest"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="request">The <see cref="HttpGetRequest"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public override async Task<T?> GetAsync<T>(HttpGetRequest request) where T : default
        {
            try
            {
                // Execute interceptors an check if request should be canceled.
                var interception = await ExecuteInterceptorsAsync(request);

                if (interception.CancelRequest)
                {
                    PerformRequestInterceptionCancellation(request, interception);

                    return default;
                }

                // Check if the result of previous request might be in cache.
                var (Cached, Result) = await TryGetCacheResultAsync<T>(request);
                if (Cached)
                    return Result;

                // Add HTTP headers defined in request.
                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);

                // Inform listeners about request to be executed.
                await OnExecutingHttpClientListenersAsync(request);

                // Perform request
                var response = await httpClient.GetAsync(request.FullRequestUri, request.CancellationToken);
                response = response.EnsureSuccessStatusCode();

                // Check if result can be cached.
                if (request.Caching.CanCacheResult)
                    await CacheResultAsync(request, response);
                
                var result = await response.Content.ReadFromJsonAsync<T>();

                // Inform listeners about executed request.
                await OnExecutedHttpClientListenersAsync(request);

                return result;
            }
            catch (Exception exception)
            {
                // Inform listeners about request error.
                await OnErrorHttpClientListenersAsync(request, exception);

                if (exception is HttpRequestException)
                    throw;

                throw new HttpRequestException(request, "The unexpected exception occurred while performing HTTP GET request.", exception);
            }
        }

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the posted data and result.</typeparam>
        /// <param name="request">The <see cref="HttpPostRequest{T}"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public override Task<T?> PostAsync<T>(HttpPostRequest<T> request) where T : default
        {
            return PostAsync<T, T>(request);
        }

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type of the posted data.</typeparam>
        /// <param name="request">The <see cref="HttpPostRequest{T}"/> to perform.</param>
        /// <returns>A <typeparamref name="TResult"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public override async Task<TResult?> PostAsync<TResult, T>(HttpPostRequest<T> request) where TResult : default
        {
            try
            {
                // Execute interceptors an check if request should be canceled.
                var interception = await ExecuteInterceptorsAsync(request);

                if (interception.CancelRequest)
                {
                    PerformRequestInterceptionCancellation(request, interception);

                    return default;
                }

                // Add HTTP headers defined in request.
                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);

                // Inform listeners about request to be executed.
                await OnExecutingHttpClientListenersAsync(request);

                // Perform request.
                var response = await httpClient.PostAsJsonAsync(request.RequestUri, request.Data, request.CancellationToken);
                response = response.EnsureSuccessStatusCode();

                // Read result
                var result = await response.Content.ReadFromJsonAsync<TResult>();

                // Inform listeners about executed request.
                await OnExecutedHttpClientListenersAsync(request);

                return result;
            }
            catch (Exception exception)
            {
                // Inform listeners about request error.
                await OnErrorHttpClientListenersAsync(request, exception);

                if (exception is HttpRequestException)
                    throw;

                throw new HttpRequestException(request, "The unexpected exception occurred while performing HTTP POST request.", exception);
            }
        }

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to perform.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public override async Task PostAsync(HttpPostRequest request)
        {
            try
            {
                // Execute interceptors an check if request should be canceled.
                var interception = await ExecuteInterceptorsAsync(request);

                if (interception.CancelRequest)
                {
                    PerformRequestInterceptionCancellation(request, interception);

                    return;
                }

                // Add HTTP headers defined in request.
                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);

                // Inform listeners about request to be executed.
                await OnExecutingHttpClientListenersAsync(request);

                // Perform request.
                var response = await httpClient.PostAsJsonAsync(request.RequestUri, request.Data, request.CancellationToken);
                response = response.EnsureSuccessStatusCode();

                // Inform listeners about executed request.
                await OnExecutedHttpClientListenersAsync(request);
            }
            catch (Exception exception)
            {
                // Inform listeners about request error.
                await OnErrorHttpClientListenersAsync(request, exception);

                if (exception is HttpRequestException)
                    throw;

                throw new HttpRequestException(request, "The unexpected exception occurred while performing HTTP POST request.", exception);
            }
        }

        private async Task CacheResultAsync(HttpGetRequest request, HttpResponseMessage response)
        {
            using (var stream = new MemoryStream())
            {
                await response.Content.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    await CacheManager.AddCacheContentAsync(request, json, ContentType, request.Caching.CacheDuration);
                }
            }
        }

        private async Task<(bool Cached, T? Result)> TryGetCacheResultAsync<T>(HttpGetRequest request)
        {
            if (request.Caching.CanCacheResult)
            {
                var cacheContent = await CacheManager.GetCacheContentAsync(request);
                if (cacheContent != null && cacheContent.ContentValue != null)
                {
                    var cacheResult = JsonSerializer.Deserialize<T>(cacheContent.ContentValue);
                    request.Caching.IsCacheResult = true;
                    return (true, cacheResult);
                }
            }

            return (false, default);
        }
    }
}
