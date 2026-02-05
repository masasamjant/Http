using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Stubs;
using Masasamjant.Xml;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http.Xml
{
    [TestClass]
    public class XmlHttpClientBuilderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Build_Contract_Serialization_Client()
        {
            var configuration = GetConfiguration(XmlSerialization.Contract);
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProviderFactory = GetHttpBaseAddressProviderFactory();
            var cacheManager = HttpCacheManager.Default;
            var builder = new XmlHttpClientBuilder(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager);
            var client = builder.Build("Test");
            Assert.IsNotNull(client);
            Assert.IsInstanceOfType<XmlHttpClient>(client);
            XmlHttpClient xclient = (XmlHttpClient)client;
            Assert.AreEqual(XmlSerialization.Contract, xclient.XmlSerialization);
        }

        [TestMethod]
        public void Test_CreateClient()
        {
            var configuration = GetConfiguration(XmlSerialization.Contract);
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProviderFactory = GetHttpBaseAddressProviderFactory();
            var cacheManager = HttpCacheManager.Default;
            var builder = new TestXmlHttpClientBuilder(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager);
            var client = builder.CreateClient("Test");
            Assert.IsNotNull(client);
            Assert.IsInstanceOfType<XmlHttpClient>(client);
            XmlHttpClient xclient = (XmlHttpClient)client;
            Assert.AreEqual(XmlSerialization.Contract, xclient.XmlSerialization);
        }

        [TestMethod]
        public void Test_ConfigureClient_Throw_If_Not_Xml_Client()
        {
            IHttpClient client = new TestHttpClient();
            var configuration = GetConfiguration(XmlSerialization.Contract);
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProviderFactory = GetHttpBaseAddressProviderFactory();
            var cacheManager = HttpCacheManager.Default;
            var builder = new TestXmlHttpClientBuilder(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager);
            Assert.ThrowsException<NotSupportedException>(() => builder.ConfigureClient(client, "Test"));
        }

        [TestMethod]
        public void Test_ConfigureClient()
        {
            var configuration = GetConfiguration(XmlSerialization.Contract);
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProviderFactory = GetHttpBaseAddressProviderFactory();
            var cacheManager = HttpCacheManager.Default;
            var builder = new TestXmlHttpClientBuilder(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager);

            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProviderFactory.GetBaseAddressProvider("Test"), cacheManager);
            client.ChangeSerialization(XmlSerialization.Xml);

            builder.ConfigureClient(client, "Test");
            Assert.AreEqual(XmlSerialization.Contract, client.XmlSerialization);

            configuration = GetConfiguration(null);
            builder = new TestXmlHttpClientBuilder(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager);

            builder.ConfigureClient(client, "Test");
            Assert.AreEqual(XmlSerialization.Contract, client.XmlSerialization);
        }

        [TestMethod]
        public void Test_Build_Xml_Serialization_Client()
        {
            var configuration = GetConfiguration(XmlSerialization.Xml);
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProviderFactory = GetHttpBaseAddressProviderFactory();
            var cacheManager = HttpCacheManager.Default;
            var builder = new XmlHttpClientBuilder(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager);
            var client = builder.Build("Test");
            Assert.IsNotNull(client);
            Assert.IsInstanceOfType<XmlHttpClient>(client);
            XmlHttpClient xclient = (XmlHttpClient)client;
            Assert.AreEqual(XmlSerialization.Xml, xclient.XmlSerialization);
        }

        private static IConfiguration GetConfiguration(XmlSerialization? serialization)
        {
            return GetConfiguration(new Dictionary<string, string?>()
            {
                { "HttpClient:Test:XmlSerialization", serialization.HasValue ? serialization.Value.ToString() : "" },
                { "HttpClient:Test:HttpBaseAddress", "http://localhost/" }
            });
        }

        private static IHttpBaseAddressProviderFactory GetHttpBaseAddressProviderFactory()
        {
            return new BasicHttpBaseAddressProviderFactory(new Dictionary<string, string>
            {
                { "Test", "http://localhost/" }
            });
        }

        private class TestXmlHttpClientBuilder : XmlHttpClientBuilder
        {
            public TestXmlHttpClientBuilder(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpBaseAddressProviderFactory httpBaseAddressProviderFactory, IHttpCacheManager? cacheManager)
                : base(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager)
            { }

            public new IHttpClient CreateClient(string clientPurpose)
            {
                return base.CreateClient(clientPurpose);
            }
            public new void ConfigureClient(IHttpClient client, string clientPurpose)
            {
                base.ConfigureClient(client, clientPurpose);
            }
        }
    }
}
