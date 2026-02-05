using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Caching;

namespace Masasamjant.Http
{
    internal class TestHttpCacheManager : HttpCacheManager
    {
        public override Task AddCacheContentAsync(HttpGetRequest request, string? contentValue, string? contentType, TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public override Task<HttpCacheContent?> GetCacheContentAsync(HttpGetRequest request)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveCacheContentAsync(HttpGetRequest request)
        {
            throw new NotImplementedException();
        }

        public string TestGetContentKey(HttpGetRequest request) => GetContentKey(request);
    }
}
