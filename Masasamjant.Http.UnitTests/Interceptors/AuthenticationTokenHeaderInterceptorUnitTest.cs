namespace Masasamjant.Http.Interceptors
{
    [TestClass]
    public class AuthenticationTokenHeaderInterceptorUnitTest : UnitTest
    {
        [TestMethod]
        public async Task Test_Constructor()
        {
            var request1 = new HttpGetRequest("/content");
            var interceptor1 = new AuthenticationTokenHeaderInterceptor("key", "value");
            Assert.IsNotNull(interceptor1);
            await interceptor1.InterceptAsync(request1);

            var request2 = new HttpGetRequest("/content");
            var interceptor2 = new AuthenticationTokenHeaderInterceptor("key", () => "value");
            Assert.IsNotNull(interceptor2);
            await interceptor2.InterceptAsync(request2);

            var request3 = new HttpGetRequest("/content");
            var interceptor3 = new AuthenticationTokenHeaderInterceptor(() => "key", () => "value");
            Assert.IsNotNull(interceptor3);
            await interceptor3.InterceptAsync(request3);

            var header1 = request1.Headers.Get("key");
            var header2 = request2.Headers.Get("key");
            var header3 = request3.Headers.Get("key");

            Assert.IsNotNull(header1);
            Assert.IsNotNull(header2);
            Assert.IsNotNull(header3);

            Assert.AreEqual(header1.Name, header2.Name);
            Assert.AreEqual(header1.Value, header2.Value);
            Assert.AreEqual(header2.Name, header3.Name);
            Assert.AreEqual(header2.Value, header3.Value);
        }

        [TestMethod]
        public async Task Test_InterceptAsync_Get()
        {
            var request = new HttpGetRequest("/content");
            var interceptor = new AuthenticationTokenHeaderInterceptor("key", "value");
            var interception = await interceptor.InterceptAsync(request);
            var header = request.Headers.Get("key");
            Assert.AreEqual(HttpRequestInterceptionResult.Continue, interception.Result);
            Assert.IsNotNull(header);
            Assert.AreEqual("key", header.Name);
            Assert.AreEqual("value", header.Value);
        }

        [TestMethod]
        public async Task Test_InterceptAsync_Post()
        {
            var request = new HttpPostRequest("/content", 1);
            var interceptor = new AuthenticationTokenHeaderInterceptor("key", "value");
            var interception = await interceptor.InterceptAsync(request);
            var header = request.Headers.Get("key");
            Assert.AreEqual(HttpRequestInterceptionResult.Continue, interception.Result);
            Assert.IsNotNull(header);
            Assert.AreEqual("key", header.Name);
            Assert.AreEqual("value", header.Value);
        }
    }
}
