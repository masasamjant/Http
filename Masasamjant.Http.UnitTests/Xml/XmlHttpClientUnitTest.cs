using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Caching;
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
        private static readonly ContractData contractData = GetContractData();
        private static readonly string contractDataXml = contractData.Serialize();
        private static readonly IHttpBaseAddressProvider httpBaseAddressProvider = GetHttpBaseAddressProvider();
        private static readonly XmlData xmlData = GetXmlData();
        private static readonly string xmlDataXml = xmlData.Serialize();
        private static readonly IHttpCacheManager httpCacheManager = HttpCacheManager.Default;

        [TestMethod]
        public void Test_Constructor()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            Assert.AreEqual(XmlSerialization.Contract, client.XmlSerialization);
        }

        [TestMethod]
        public void Test_ChangeSerialization()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            Assert.AreEqual(XmlSerialization.Contract, client.XmlSerialization);
            client.ChangeSerialization(XmlSerialization.Xml);
            Assert.AreEqual(XmlSerialization.Xml, client.XmlSerialization);
            Assert.ThrowsException<ArgumentException>(() => client.ChangeSerialization((XmlSerialization)999));
        }

        [TestMethod]
        public async Task Test_GetAsync_Cancel()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
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
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
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
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            client.HttpClientListeners.Add(listener);
            var request = new HttpPostRequest<ContractData>("/contract", contractData);
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
        public async Task Test_PostAsync_Exception_Handling()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            handler.Exception = new Exception("Test");
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => 
            {
                var request1 = new HttpPostRequest<ContractData>("/contract", contractData);
                return client.PostAsync(request1);
            });

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => 
            {
                var request2 = new HttpPostRequest("/contract", contractData);
                return client.PostAsync(request2);
            });
        }

        [TestMethod]
        public async Task Test_PostAsync_Cancel()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Test"), new List<string>()));
            bool requestCanceled = false;
            var request = new HttpPostRequest<ContractData>("/contract", contractData);
            request.Canceled += (s, e) => { requestCanceled = true; };
            var result = await client.PostAsync(request);
            Assert.IsNull(result);
            Assert.IsTrue(requestCanceled);
        }

        [TestMethod]
        public async Task Test_Contract_GetAsync()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpGetRequest("/contract");
            var result = await client.GetAsync<ContractData>(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(contractData.Name, result.Name);
            Assert.AreEqual(contractData.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.GetAsync<ContractData>(request));
        }

        [TestMethod]
        public async Task Test_Xml_GetAsync()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xmlDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            client.ChangeSerialization(XmlSerialization.Xml);
            var request = new HttpGetRequest("/contract");
            var result = await client.GetAsync<XmlData>(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(xmlData.Name, result.Name);
            Assert.AreEqual(xmlData.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.GetAsync<XmlData>(request));
        }

        [TestMethod]
        public async Task Test_Contract_PostAsync()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpPostRequest<ContractData>("/contract", contractData);
            var result = await client.PostAsync(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(contractData.Name, result.Name);
            Assert.AreEqual(contractData.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.PostAsync(request));
        }

        [TestMethod]
        public async Task Test_Xml_PostAsync()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xmlDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpPostRequest<XmlData>("/contract", xmlData);
            var result = await client.PostAsync(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(xmlData.Name, result.Name);
            Assert.AreEqual(xmlData.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.PostAsync(request));
        }

        [TestMethod]
        public async Task Test_Xml_PostAsync_No_Result()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            HttpPostRequest request = new HttpPostRequest("/contract", contractData);
            await client.PostAsync(request);

            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Test"), new List<string>()));
            bool requestCanceled = false;
            request.Canceled += (s, e) => { requestCanceled = true; };
            await client.PostAsync(request);
            Assert.IsTrue(requestCanceled);

            client = new TestXmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager, true);
            await client.PostAsync(request);
        }

        [TestMethod]
        public async Task Test_PostAsync_Return_Default()
        {
            var data = GetXmlData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new TestXmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager, true);
            var request = new HttpPostRequest<XmlData>("/contract", data);
            var result = await client.PostAsync(request);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Test_GetAsync_Cache()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpCacheManager = new MemoryHttpCacheManager();
            var client = new XmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var caching = new HttpGetRequestCaching(true, TimeSpan.FromMinutes(2));
            var request = new HttpGetRequest("/contract", caching);
            Assert.IsTrue(request.Caching.CanCacheResult);
            
            var result = await client.GetAsync<ContractData>(request);
            Assert.IsFalse(request.Caching.IsCacheResult);
            Assert.IsNotNull(result);

            request = new HttpGetRequest("/contract", caching);
            result = await client.GetAsync<ContractData>(request);

            Assert.IsTrue(request.Caching.IsCacheResult);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test_DeserializeCacheContentValue()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, contractDataXml, null, "application/xml");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var client = new TestXmlHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var result = client.DeserializeCacheContentValue<ContractData>(contractDataXml);
            Assert.IsNotNull(result);
            Assert.AreEqual(contractData.Name, result.Name);
            result = client.DeserializeCacheContentValue<ContractData>(null);
            Assert.IsNull(result);
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

        private class TestXmlHttpClient : XmlHttpClient
        {
            public TestXmlHttpClient(IHttpClientFactory httpClientFactory, IHttpBaseAddressProvider httpBaseAddressProvider, IHttpCacheManager httpCacheManager, bool serializeXmlReturnNull = false)
                : base(httpClientFactory, httpBaseAddressProvider, httpCacheManager)
            {
                this.serializeXmlReturnNull = serializeXmlReturnNull;
            }

            private readonly bool serializeXmlReturnNull = false;

            public new T? DeserializeCacheContentValue<T>(string? contentValue)
            {
                return base.DeserializeCacheContentValue<T>(contentValue);
            }

            protected override string? SerializeXml(object instance)
            {
                if (serializeXmlReturnNull)
                    return null;

                return base.SerializeXml(instance);
            }
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
