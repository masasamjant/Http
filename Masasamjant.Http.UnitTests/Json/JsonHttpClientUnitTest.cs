using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Caching;
using Masasamjant.Http.Stubs;
using Masasamjant.Http.Xml;
using System.Text;
using System.Text.Json;

namespace Masasamjant.Http.Json
{
    [TestClass]
    public class JsonHttpClientUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, string.Empty, null, null);
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            Assert.IsNotNull(client);
        }

        [TestMethod]
        public async Task Test_GetAsync_Cancel()
        {
            var data = GetJsonData();
            var json = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, json, null, "application/json");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            client.HttpGetRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Test"), new List<string>()));
            bool requestCanceled = false;
            var request = new HttpGetRequest("/contract");
            request.Canceled += (s, e) => { requestCanceled = true; };
            var result = await client.GetAsync<ContractData>(request);
            Assert.IsNull(result);
            Assert.IsTrue(requestCanceled);
        }

        [TestMethod]
        public async Task Test_PostAsync_Cancel()
        {
            var data = GetJsonData();
            var xml = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, xml, null, "application/xmljson");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Test"), new List<string>()));
            bool requestCanceled = false;
            var request = new HttpPostRequest<JsonData>("/contract", data);
            request.Canceled += (s, e) => { requestCanceled = true; };
            var result = await client.PostAsync(request);
            Assert.IsNull(result);
            Assert.IsTrue(requestCanceled);
        }

        [TestMethod]
        public async Task Test_GetAsync_Listener_Execution()
        {
            var data = GetJsonData();
            var json = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, json, null, "application/json");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            client.HttpClientListeners.Add(listener);
            var request = new HttpGetRequest("/contract");
            var result = await client.GetAsync<JsonData>(request);
            string s = builder.ToString();
            Assert.IsTrue(s.Contains("Executing: " + request.Identifier));
            Assert.IsTrue(s.Contains("Executed: " + request.Identifier));
            builder.Clear();
            handler.Exception = new Exception("Test");
            try
            {
                await client.GetAsync<JsonData>(request);
            }
            catch (HttpRequestException) { }
            s = builder.ToString();
            Assert.IsTrue(s.Contains("Executing: " + request.Identifier));
            Assert.IsTrue(s.Contains("Error: Test"));
        }

        [TestMethod]
        public async Task Test_PostAsync_Listener_Execution()
        {
            var data = GetJsonData();
            var json = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, json, null, "application/json");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            client.HttpClientListeners.Add(listener);
            var request = new HttpPostRequest<JsonData>("/contract", data);
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
        public async Task Test_GetAsync()
        {
            var data = GetJsonData();
            var json = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, json, null, "application/json");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpGetRequest("/contract");
            var result = await client.GetAsync<JsonData>(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(data.Name, result.Name);
            Assert.AreEqual(data.Age, result.Age);
            handler.Exception = new InvalidOperationException("Testing");
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => client.GetAsync<JsonData>(request));
        }

        [TestMethod]
        public async Task Test_GetAsync_Cache()
        {
            var data = GetJsonData();
            var json = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, json, null, "application/json");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = new MemoryHttpCacheManager();
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
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
        public async Task Test_PostAsync()
        {
            var data = GetJsonData();
            var json = data.Serialize();
            var handler = new HttpMessageHandlerStub(System.Net.HttpStatusCode.OK, json, null, "application/json");
            var httpClientFactory = new HttpClientFactoryStub(handler);
            var httpBaseAddressProvider = GetHttpBaseAddressProvider();
            var httpCacheManager = HttpCacheManager.Default;
            var client = new JsonHttpClient(httpClientFactory, httpBaseAddressProvider, httpCacheManager);
            var request = new HttpPostRequest<JsonData>("/contract", data);
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

        private static JsonData GetJsonData()
        {
            return new JsonData()
            {
                Name = "Mike",
                Age = 10
            };
        }
    }

    public class JsonData
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; } = 0;

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
