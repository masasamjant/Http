using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Stubs;

namespace Masasamjant.Http.Json
{
    [TestClass]
    public class JsonHttpClientBuilderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Build()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>());
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProviderFactory = GetHttpBaseAddressProviderFactory();
            var cacheManager = HttpCacheManager.Default;
            var builder = new JsonHttpClientBuilder(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager);
            var client = builder.Build("Test");
            Assert.IsNotNull(client);
            Assert.IsInstanceOfType<JsonHttpClient>(client);
        }

        private static IHttpBaseAddressProviderFactory GetHttpBaseAddressProviderFactory()
        {
            return new BasicHttpBaseAddressProviderFactory(new Dictionary<string, string>
            {
                { "Test", "http://localhost/" }
            });
        }
    }
}
