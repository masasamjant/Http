namespace Masasamjant.Http.Abstractions
{
    [TestClass]
    public class HttpClientBuilderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var builder = new TestHttpClientBuilder();
            Assert.ThrowsException<InvalidOperationException>(() => builder.HttpClientFactory);
            Assert.ThrowsException<InvalidOperationException>(() => builder.HttpBaseAddressProviderFactory);
            Assert.ThrowsException<InvalidOperationException>(() => builder.Configuration);
            var client = builder.Build("Test") as TestHttpClient;
            Assert.IsNotNull(client);
            Assert.IsTrue(client.IsConfigured);
        }
    }
}
