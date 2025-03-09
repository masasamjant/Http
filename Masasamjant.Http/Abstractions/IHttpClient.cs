using Masasamjant.Http.Interceptors;
using Masasamjant.Http.Listeners;

namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents HTTP client.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Gets the <see cref="HttpGetRequestInterceptorCollection"/> for interceptors executed 
        /// when HTTP GET request is performed.
        /// </summary>
        HttpGetRequestInterceptorCollection HttpGetRequestInterceptors { get; }

        /// <summary>
        /// Gets the <see cref="HttpPostRequestInterceptorCollection"/> for interceptors executed 
        /// when HTTP POST request is performed.
        /// </summary>
        HttpPostRequestInterceptorCollection HttpPostRequestInterceptors { get; }

        /// <summary>
        /// Gets the <see cref="HttpClientListenerCollection"/> for listeners of HTTP request execution.
        /// </summary>
        HttpClientListenerCollection HttpClientListeners { get; }

        /// <summary>
        /// Perform HTTP GET request using specified <see cref="HttpGetRequest"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="request">The <see cref="HttpGetRequest"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        Task<T?> GetAsync<T>(HttpGetRequest request);

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the posted data and result.</typeparam>
        /// <param name="request">The <see cref="HttpPostRequest{T}"/> to perform.</param>
        /// <returns>A <typeparamref name="T"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        Task<T?> PostAsync<T>(HttpPostRequest<T> request) where T : notnull;

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type of the posted data.</typeparam>
        /// <param name="request">The <see cref="HttpPostRequest{T}"/> to perform.</param>
        /// <returns>A <typeparamref name="TResult"/> result of request or default.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        Task<TResult?> PostAsync<TResult, T>(HttpPostRequest<T> request) where T : notnull;

        /// <summary>
        /// Perform HTTP POST request using specified <see cref="HttpPostRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to perform.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <exception cref="HttpRequestException">If exception occurs when executing request.</exception>
        Task PostAsync(HttpPostRequest request);
    }
}
