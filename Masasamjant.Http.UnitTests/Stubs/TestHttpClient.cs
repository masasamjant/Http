using Masasamjant.Http.Abstractions;
using System.Net.Http.Headers;
using HttpClient = Masasamjant.Http.Abstractions.HttpClient;

namespace Masasamjant.Http
{
    internal class TestHttpClient : HttpClient
    {
        public bool IsConfigured { get; internal set; }

        public override Task<T?> GetAsync<T>(HttpGetRequest request) where T : default
        {
            throw new NotImplementedException();
        }

        public override Task<T?> PostAsync<T>(HttpPostRequest<T> request) where T : default
        {
            throw new NotImplementedException();
        }

        public override Task<TResult?> PostAsync<TResult, T>(HttpPostRequest<T> request) where TResult : default
        {
            throw new NotImplementedException();
        }

        public override Task PostAsync(HttpPostRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task TestOnExecutingHttpClientListenersAsync(HttpRequest request) => await OnExecutingHttpClientListenersAsync(request);

        public async Task TestOnExecutedHttpClientListenersAsync(HttpRequest request) => await OnExecutedHttpClientListenersAsync(request);

        public async Task TestOnErrorHttpClientListenersAsync(HttpRequest request, Exception exception) => await OnErrorHttpClientListenersAsync(request, exception);

        public static void TestAddHttpHeaders(HttpRequest request, HttpHeaders headers) => AddHttpHeaders(request, headers);

        public async Task<bool> TestIsCanceledByInterceptorsAsync(HttpGetRequest request) => await IsCanceledByInterceptorsAsync(request);

        public async Task<bool> TestIsCanceledByInterceptorsAsync(HttpPostRequest request) => await IsCanceledByInterceptorsAsync(request);

        protected override T? DeserializeCacheContentValue<T>(string? contentValue) where T : default
        {
            throw new NotImplementedException();
        }

        public async Task<HttpRequestException> TestHandleRequestExceptionAsync(HttpRequest request, Exception exception, string message) => await HandleRequestExceptionAsync(request, exception, message);
    }
}
