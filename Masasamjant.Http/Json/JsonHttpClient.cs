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
    public class JsonHttpClient : HttpClient
    {
        private readonly System.Net.Http.HttpClient httpClient;
        private const string ContentType = "application/json";

        /// <summary>
        /// Initializes new instance of the <see cref="JsonHttpClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="httpBaseAddressProvider">The <see cref="IHttpBaseAddressProvider"/>.</param>
        /// <param name="httpCacheManager">The <see cref="IHttpCacheManager"/>.</param>
        public JsonHttpClient(IHttpClientFactory httpClientFactory, IHttpBaseAddressProvider httpBaseAddressProvider, IHttpCacheManager httpCacheManager)
            : base(httpCacheManager)
        {
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(httpBaseAddressProvider.GetHttpBaseAdress());
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
        }

        /// <summary>
        /// Perform HTTP GET request using specified <see cref="HttpGetRequest"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="request">The <see cref="HttpGetRequest"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public override sealed async Task<T?> GetAsync<T>(HttpGetRequest request) where T : default
        {
            try
            {
                // Execute interceptors an check if request should be canceled.
                if (await IsCanceledByInterceptorsAsync(request))
                    return default;

                // Check if the result of previous request might be in cache.
                var (Cached, Result) = await TryGetCacheResultAsync<T>(request);
                if (Cached)
                    return Result;

                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);
                await OnExecutingHttpClientListenersAsync(request);
                var response = await PerformGetRequestAsync(request);
                await CacheResultAsync(request, response, ContentType);
                var result = await response.Content.ReadFromJsonAsync<T>();
                await OnExecutedHttpClientListenersAsync(request);
                return result;
            }
            catch (Exception exception)
            {
                throw await HandleRequestExceptionAsync(request, exception, "The unexpected exception occurred while performing HTTP GET request.");
            }
        }

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the posted data and result.</typeparam>
        /// <param name="request">The <see cref="HttpPostRequest{T}"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public override sealed Task<T?> PostAsync<T>(HttpPostRequest<T> request) where T : default
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
        public override sealed async Task<TResult?> PostAsync<TResult, T>(HttpPostRequest<T> request) where TResult : default
        {
            try
            {
                // Execute interceptors an check if request was canceled.
                if (await IsCanceledByInterceptorsAsync(request))
                    return default;

                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);
                await OnExecutingHttpClientListenersAsync(request);
                var response = await PerformPostRequestAsync(request);
                var result = await response.Content.ReadFromJsonAsync<TResult>();
                await OnExecutedHttpClientListenersAsync(request);
                return result;
            }
            catch (Exception exception)
            {
                throw await HandleRequestExceptionAsync(request, exception, "The unexpected exception occurred while performing HTTP POST request.");
            }
        }

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to perform.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        public override sealed async Task PostAsync(HttpPostRequest request)
        {
            try
            {
                // Execute interceptors an check if request should be canceled.
                if (await IsCanceledByInterceptorsAsync(request))
                    return;

                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);
                await OnExecutingHttpClientListenersAsync(request);
                var response = await PerformPostRequestAsync(request);
                await OnExecutedHttpClientListenersAsync(request);
            }
            catch (Exception exception)
            {
                throw await HandleRequestExceptionAsync(request, exception, "The unexpected exception occurred while performing HTTP POST request.");
            }
        }

        /// <summary>
        /// Deserialize cache content value.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object.</typeparam>
        /// <param name="contentValue">The cache content value to deserialize.</param>
        /// <returns>A deserialized value.</returns>
        protected override T? DeserializeCacheContentValue<T>(string? contentValue) where T: default
        {
            if (contentValue == null)
                return default;

            return JsonSerializer.Deserialize<T>(contentValue);
        }

        private async Task<HttpResponseMessage> PerformGetRequestAsync(HttpGetRequest request)
        {
            var response = await httpClient.GetAsync(request.FullRequestUri, request.CancellationToken);
            return response.EnsureSuccessStatusCode();
        }

        private async Task<HttpResponseMessage> PerformPostRequestAsync(HttpPostRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(request.RequestUri, request.Data, request.CancellationToken);
            return response.EnsureSuccessStatusCode();
        }
    }
}
