namespace Masasamjant.Http.Abstractions
{
    [TestClass]
    public class HttpClientBuilderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var builder = new TestHttpClientBuilder(null, null, null, null);
            Assert.ThrowsException<InvalidOperationException>(() => builder.HttpClientFactory);
            Assert.ThrowsException<InvalidOperationException>(() => builder.HttpBaseAddressProviderFactory);
            Assert.ThrowsException<InvalidOperationException>(() => builder.Configuration);

            var client = builder.Build("Test") as TestHttpClient;
            Assert.IsNotNull(client);
            Assert.IsTrue(client.IsConfigured);

            builder = new TestHttpClientBuilder();
            Assert.ThrowsException<InvalidOperationException>(() => builder.HttpClientFactory);
            Assert.ThrowsException<InvalidOperationException>(() => builder.HttpBaseAddressProviderFactory);
            Assert.ThrowsException<InvalidOperationException>(() => builder.Configuration);
            var manager = builder.CacheManager;
            Assert.IsNotNull(manager);
            Assert.AreSame(HttpCacheManager.Default, manager);
        }
    }
}
