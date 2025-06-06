using HttpClient = Masasamjant.Http.Abstractions.HttpClient;
using Masasamjant.Http.Abstractions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Masasamjant.Http.Json
{
    /// <summary>
    /// Represents HTTP client that accepts JSON data.
    /// </summary>
    public sealed class JsonHttpClient : HttpClient
    {
        private readonly System.Net.Http.HttpClient httpClient;

        /// <summary>
        /// Initializes new instance of the <see cref="JsonHttpClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="httpBaseAddressProvider">The <see cref="IHttpBaseAddressProvider"/>.</param>
        public JsonHttpClient(IHttpClientFactory httpClientFactory, IHttpBaseAddressProvider httpBaseAddressProvider)
        {
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(httpBaseAddressProvider.GetHttpBaseAdress());
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

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
                    request.Cancel();

                    if (interception.ThrowCancelException)
                        throw GetInterceptionException(request, interception);

                    return default;
                }

                // Add HTTP headers defined in request.
                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);

                // Inform listeners about request to be executed.
                await OnExecutingHttpClientListenersAsync(request);

                // Perform request
                var response = await httpClient.GetAsync(request.FullRequestUri, request.CancellationToken);
                response = response.EnsureSuccessStatusCode();

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
                    request.Cancel();

                    if (interception.ThrowCancelException)
                        throw GetInterceptionException(request, interception);

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
                    request.Cancel();

                    if (interception.ThrowCancelException)
                        throw GetInterceptionException(request, interception);

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
    }
}
