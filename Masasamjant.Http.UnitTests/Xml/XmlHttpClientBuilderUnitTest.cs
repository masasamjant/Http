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

        private static IConfiguration GetConfiguration(XmlSerialization serialization)
        {
            return GetConfiguration(new Dictionary<string, string?>()
            {
                { "HttpClient:Test:XmlSerialization", serialization.ToString() },
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
    }
}
