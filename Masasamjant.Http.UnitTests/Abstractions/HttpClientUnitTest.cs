using Masasamjant.Http.Interceptors;
using System.Text;

namespace Masasamjant.Http.Abstractions
{
    [TestClass]
    public class HttpClientUnitTest : UnitTest
    {
        [TestMethod]
        public async Task Test_ExecuteInterceptorsAsync_With_Get()
        {
            var client = new TestHttpClient();
            var lines = new List<string>();
            client.HttpGetRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines));
            client.HttpGetRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines));
            var request = new HttpGetRequest("api/Test");
            var interception = await client.TestExecuteInterceptorsAsync(request);
            Assert.AreEqual(2, lines.Count);
            Assert.IsTrue(interception.Result == HttpRequestInterceptionResult.Continue);
            lines.Clear();
            client.HttpGetRequestInterceptors.Clear();
            client.HttpGetRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Testing"), lines));
            client.HttpGetRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines));
            interception = await client.TestExecuteInterceptorsAsync(request);
            Assert.AreEqual(1, lines.Count);
            Assert.IsTrue(interception.Result == HttpRequestInterceptionResult.Cancel);
        }

        [TestMethod]
        public async Task Test_ExecuteInterceptorsAsync_With_Post()
        {
            var client = new TestHttpClient();
            var lines = new List<string>();
            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines));
            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines));
            var request = new HttpPostRequest("api/Test", DateTime.Now);
            var interception = await client.TestExecuteInterceptorsAsync(request);
            Assert.AreEqual(2, lines.Count);
            Assert.IsTrue(interception.Result == HttpRequestInterceptionResult.Continue);
            lines.Clear();
            client.HttpPostRequestInterceptors.Clear();
            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Testing"), lines));
            client.HttpPostRequestInterceptors.Add(new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines));
            interception = await client.TestExecuteInterceptorsAsync(request);
            Assert.AreEqual(1, lines.Count);
            Assert.IsTrue(interception.Result == HttpRequestInterceptionResult.Cancel);
        }

        [TestMethod]
        public async Task Test_OnExecutingHttpClientListenersAsync()
        {
            var builder = new StringBuilder();
            var client = new TestHttpClient();
            var listener = new TestHttpClientListener(builder);
            client.HttpClientListeners.Add(listener);
            var request = new HttpGetRequest("api/Test");
            await client.TestOnExecutingHttpClientListenersAsync(request);
            string expected = $"Executing: {request.Identifier}";
            string actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Test_OnErrorHttpClientListenersAsync()
        {
            var builder = new StringBuilder();
            var client = new TestHttpClient();
            var listener = new TestHttpClientListener(builder);
            client.HttpClientListeners.Add(listener);
            var request = new HttpGetRequest("api/Test");
            var exception = new InvalidOperationException("exception");
            await client.TestOnErrorHttpClientListenersAsync(request, exception);
            string expected = $"Error: {exception.Message}";
            string actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_AddHttpHeaders()
        {
            var request = new HttpGetRequest("api/Test");
            var headers = new TestHttpHeaders();
            headers.Add("name", "value");
            TestHttpClient.TestAddHttpHeaders(request, headers);
            Assert.IsTrue(headers.Contains("name"));
            Assert.IsTrue(headers.GetValues("name").Contains("value"));

            request.Headers.Add("name", "name");
            request.Headers.Add("test", "test");
            TestHttpClient.TestAddHttpHeaders(request, headers);
            Assert.IsTrue(headers.Contains("name"));
            Assert.IsTrue(headers.Contains("test"));
            Assert.IsFalse(headers.GetValues("name").Contains("value"));
            Assert.IsTrue(headers.GetValues("name").Contains("name"));
            Assert.IsTrue(headers.GetValues("test").Contains("test"));
        }

        [TestMethod]
        public void Test_PerformRequestInterceptionCancellation()
        {
            bool canceled = false;
            var request = new HttpGetRequest("api/Test");
            request.Canceled += (s, e) => { canceled = true; };
            Assert.ThrowsException<ArgumentException>(() => {
                TestHttpClient.TestPerformRequestInterceptionCancellation(request, HttpRequestInterception.Continue);
            });
            Assert.ThrowsException<HttpRequestInterceptionException>(() => {
                TestHttpClient.TestPerformRequestInterceptionCancellation(request, HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Throw, "Testing"));
            });
            Assert.IsTrue(canceled);
            canceled = false;
            request = new HttpGetRequest("api/Test");
            request.Canceled += (s, e) => { canceled = true; };
            TestHttpClient.TestPerformRequestInterceptionCancellation(request, HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Testing"));
            Assert.IsTrue(canceled);
        }
    }
}
