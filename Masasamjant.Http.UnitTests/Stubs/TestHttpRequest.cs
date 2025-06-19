using Masasamjant.Http.Abstractions;

namespace Masasamjant.Http
{
    internal class TestHttpRequest : HttpRequest
    {
        public TestHttpRequest(string requestUri, HttpRequestMethod requestMethod) 
            : base(requestUri, requestMethod)
        { }

        public void TestCloneHeadersTo(HttpRequest request)
        {
            base.CloneHeadersTo(request);
        }
    }
}
