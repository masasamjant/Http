using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http
{
    internal class TestHttpRequestInterceptor : HttpRequestInterceptor
    {
        private readonly HttpRequestInterception interception;
        private readonly List<string> lines;

        public TestHttpRequestInterceptor(HttpRequestInterception interception, List<string> lines)
        {
            this.interception = interception;
            this.lines = lines;
        }

        public override Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request)
        {
            lines.Add($"GET: {request.Identifier}");
            return Task.FromResult(interception);
        }

        public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
        {
            lines.Add($"POST: {request.Identifier}");
            return Task.FromResult(interception);
        }
    }
}
