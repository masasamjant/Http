using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Stubs;
using Masasamjant.Xml;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Masasamjant.Http.Xml
{
    [TestClass]
    public class XmlHttpClientUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            Assert.AreEqual(XmlSerialization.Contract, client.XmlSerialization);
        }

        [TestMethod]
        public void Test_ChangeSerialization()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            Assert.AreEqual(XmlSerialization.Contract, client.XmlSerialization);
            client.ChangeSerialization(XmlSerialization.Xml);
            Assert.AreEqual(XmlSerialization.Xml, client.XmlSerialization);
            Assert.ThrowsException<ArgumentException>(() => client.ChangeSerialization((XmlSerialization)999));
        }

        [TestMethod]
        public async Task Test_GetAsync_Cancel()
        {
            var data = GetContractData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            client.HttpGetRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Test"), new List<string>()));
            bool requestCanceled = false;
            var request = new HttpGetRequest("/contract");
            request.Canceled += (s, e) => { requestCanceled = true; };
            var result = await client.GetAsync<ContractData>(request);
            Assert.IsNull(result);
            Assert.IsTrue(requestCanceled);
        }

        [TestMethod]
        public async Task Test_GetAsync_Listener_Execution()
        {
            var data = GetContractData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            client.HttpClientListeners.Add(listener);
            var request = new HttpGetRequest("/contract");
            var result = await client.GetAsync<ContractData>(request);
            string s = builder.ToString();
            Assert.IsTrue(s.Contains("Executing: " + request.Identifier));
            Assert.IsTrue(s.Contains("Executed: " + request.Identifier));
            builder.Clear();
            handler.Exception = new Exception("Test");
            try
            {
                await client.GetAsync<ContractData>(request);
            }
            catch (HttpRequestException) { }
            s = builder.ToString();
            Assert.IsTrue(s.Contains("Executing: " + request.Identifier));
            Assert.IsTrue(s.Contains("Error: Test"));
        }

        [TestMethod]
        public async Task Test_PostAsync_Listener_Execution()
        {
            var data = GetContractData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            client.HttpClientListeners.Add(listener);
            var request = new HttpPostRequest<ContractData>("/contract", data);
            var result = await client.PostAsync(request);
            string s = builder.ToString();
            Assert.IsTrue(s.Contains("Executing: " + request.Identifier));
            Assert.IsTrue(s.Contains("Executed: " + request.Identifier));
            builder.Clear();
            handler.Exception = new Exception("Test");
            try
            {
                await client.PostAsync(request);
            }
            catch (HttpRequestException) { }
            s = builder.ToString();
            Assert.IsTrue(s.Contains("Executing: " + request.Identifier));
            Assert.IsTrue(s.Contains("Error: Test"));
        }

        [TestMethod]
        public async Task Test_PostAsync_Cancel()
        {
            var data = GetContractData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Test"), new List<string>()));
            bool requestCanceled = false;
            var request = new HttpPostRequest<ContractData>("/contract", data);
            request.Canceled += (s, e) => { requestCanceled = true; };
            var result = await client.PostAsync(request);
            Assert.IsNull(result);
            Assert.IsTrue(requestCanceled);
        }

        [TestMethod]
        public async Task Test_Contract_GetAsync()
        {
            var data = GetContractData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpGetRequest("/contract");
            var result = await client.GetAsync<ContractData>(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(data.Name, result.Name);
            Assert.AreEqual(data.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.GetAsync<ContractData>(request));
        }

        [TestMethod]
        public async Task Test_Xml_GetAsync()
        {
            var data = GetXmlData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            client.ChangeSerialization(XmlSerialization.Xml);
            var request = new HttpGetRequest("/contract");
            var result = await client.GetAsync<XmlData>(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(data.Name, result.Name);
            Assert.AreEqual(data.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.GetAsync<XmlData>(request));
        }

        [TestMethod]
        public async Task Test_Contract_PostAsync()
        {
            var data = GetContractData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpPostRequest<ContractData>("/contract", data);
            var result = await client.PostAsync(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(data.Name, result.Name);
            Assert.AreEqual(data.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.PostAsync(request));
        }

        [TestMethod]
        public async Task Test_Xml_PostAsync()
        {
            var data = GetXmlData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpPostRequest<XmlData>("/contract", data);
            var result = await client.PostAsync(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(data.Name, result.Name);
            Assert.AreEqual(data.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.PostAsync(request));
        }

        private static IHttpBaseAddressProvider GetHttpBaseAddressProvider()
        {
            var factory = new BasicHttpBaseAddressProviderFactory(new Dictionary<string, string>
            {
                { "Test", "http://localhost/" }
            });

            return factory.GetBaseAddressProvider("Test");
        }

        private static ContractData GetContractData()
        {
            return new ContractData()
            {
                Name = "Mike",
                Age = 10
            };
        }

        private static XmlData GetXmlData()
        {
            return new XmlData()
            {
                Name = "Mike",
                Age = 10
            };
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XmlData
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement(ElementName = "age")]
        public int Age { get; set; } = 0;

        internal string Serialize()
        {
            var serializer = new XmlDataSerializer(typeof(XmlData));
            return serializer.Serialize(this);
        }
    }

    [DataContract]
    public class ContractData
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public int Age { get; set; } = 0;

        internal string Serialize()
        {
            var serializer = new XmlDataContractSerializer(typeof(ContractData));
            return serializer.Serialize(this);
        }
    }
}
