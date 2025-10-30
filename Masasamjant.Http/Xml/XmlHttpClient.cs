﻿using HttpClient = Masasamjant.Http.Abstractions.HttpClient;
using Masasamjant.Http.Abstractions;
using System.Net.Http.Headers;
using System.Xml;
using Masasamjant.Xml;

namespace Masasamjant.Http.Xml
{
    /// <summary>
    /// Represents HTTP client that accepts XML data.
    /// </summary>
    public sealed class XmlHttpClient : HttpClient
    {
        private readonly System.Net.Http.HttpClient httpClient;
        private const string ContentType = "application/xml";

        /// <summary>
        /// Initializes new instance of the <see cref="XmlHttpClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="httpBaseAddressProvider">The <see cref="IHttpBaseAddressProvider"/>.</param>
        /// <param name="httpCacheManager">The <see cref="IHttpCacheManager"/>.</param>
        public XmlHttpClient(IHttpClientFactory httpClientFactory, IHttpBaseAddressProvider httpBaseAddressProvider, IHttpCacheManager httpCacheManager)
            : base(httpCacheManager)
        {
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(httpBaseAddressProvider.GetHttpBaseAdress());
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
            XmlSerialization = XmlSerialization.Contract;
        }

        /// <summary>
        /// Gets what XML serialization is performed.
        /// </summary>
        public XmlSerialization XmlSerialization { get; internal set; }

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
                if (await IsCanceledByInterceptorsAsync(request))
                    return default;

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

                // Caches results if possible.
                await CacheResultAsync(request, response, ContentType);

                var result = await response.Content.ReadAsStringAsync();

                // Inform listeners about executed request.
                await OnExecutedHttpClientListenersAsync(request);

                return DeserializeXml<T>(result);
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
                // Execute interceptors and check if request was canceled.
                if (await IsCanceledByInterceptorsAsync(request))
                    return default;
             
                // Serialize request data to XML.
                var xml = SerializeXml(request.Data);

                if (xml == null)
                    return default;

                // Add HTTP headers defined in request.
                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);

                // Inform listeners about request to be executed.
                await OnExecutingHttpClientListenersAsync(request);

                // Perform request.
                var response = await httpClient.PostAsync(request.RequestUri, new StringContent(xml), request.CancellationToken);
                response = response.EnsureSuccessStatusCode();

                // Read result.
                var result = await response.Content.ReadAsStringAsync();

                // Inform listeners about executed request.
                await OnExecutedHttpClientListenersAsync(request);

                return DeserializeXml<TResult>(result);
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
                // Execute interceptors and check if request was canceled.
                if (await IsCanceledByInterceptorsAsync(request))
                    return;

                // Serialize request data to XML.
                // Since data could be anything, reconsider adding check if data is string or XML document etc.,
                // but for now just attempt to serialize to XML.
                var xml = SerializeXml(request.Data);

                if (xml == null)
                    return;

                // Add HTTP headers defined in request.
                AddHttpHeaders(request, httpClient.DefaultRequestHeaders);

                // Inform listeners about request to be executed.
                await OnExecutingHttpClientListenersAsync(request);

                // Perform request.
                var response = await httpClient.PostAsync(request.RequestUri, new StringContent(xml), request.CancellationToken);
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

        /// <summary>
        /// Deserialize cache content value.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object.</typeparam>
        /// <param name="contentValue">The cache content value to deserialize.</param>
        /// <returns>A deserialized value.</returns>
        protected override T? DeserializeCacheContentValue<T>(string? contentValue) where T : default
        {
            return DeserializeXml<T>(contentValue);
        }

        private T? DeserializeXml<T>(string? xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return default;

            var document = new XmlDocument();
            document.LoadXml(xml);

            var factory = new XmlSerializerFactory(XmlSerialization);
            var serializer = factory.CreateSerializer(typeof(T));
            return serializer.Deserialize<T>(document);
        }

        private string? SerializeXml(object instance)
        {
            var factory = new XmlSerializerFactory(XmlSerialization);
            var serializer = factory.CreateSerializer(instance.GetType());
            var xml = serializer.Serialize(instance);
            return xml;
        }
    }
}
