using System.Net;
using System.Text;

namespace Masasamjant.Http.Stubs
{
    internal class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly HttpStatusCode statusCode;
        private readonly HttpContent content;

        public HttpMessageHandlerStub(HttpStatusCode statusCode, HttpContent content)
        {
            this.statusCode = statusCode;
            this.content = content;
        }

        public HttpMessageHandlerStub(HttpStatusCode statusCode, string? content, Encoding? encoding, string? mediaType)
            : this(statusCode, new StringContent(content ?? string.Empty, encoding, mediaType))
        { }

        public Exception? Exception { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Exception != null)
                throw Exception;

            HttpResponseMessage response;

            if (request.Method == HttpMethod.Get)
                response = SendGet(request);
            else
                response = SendPost(request);

            return Task.FromResult(response);
        }

        private HttpResponseMessage SendGet(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(statusCode);
            response.Content = content;
            return response;
        }

        private HttpResponseMessage SendPost(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(statusCode);
            response.Content = request.Content;
            return response;
        }
    }
}
