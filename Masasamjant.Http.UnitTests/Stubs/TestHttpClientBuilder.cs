using Masasamjant.Http.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http
{
    internal class TestHttpClientBuilder : HttpClientBuilder
    {
        public TestHttpClientBuilder(IConfiguration? configuration = null, IHttpClientFactory? httpClientFactory = null, IHttpBaseAddressProviderFactory? httpBaseAddressProviderFactory = null, IHttpCacheManager? httpCacheManager = null)
            : base(configuration, httpClientFactory, httpBaseAddressProviderFactory, httpCacheManager)  
        { }

        public new IHttpClientFactory HttpClientFactory => base.HttpClientFactory;

        public new IHttpBaseAddressProviderFactory HttpBaseAddressProviderFactory => base.HttpBaseAddressProviderFactory;

        public new IConfiguration Configuration => base.Configuration;

        public new IHttpCacheManager CacheManager => base.CacheManager;

        protected override IHttpClient CreateClient(string clientPurpose)
        {
            return new TestHttpClient();
        }

        protected override void ConfigureClient(IHttpClient client, string clientPurpose)
        {
            if (client is TestHttpClient testClient)
                testClient.IsConfigured = true;
        }
    }
}
