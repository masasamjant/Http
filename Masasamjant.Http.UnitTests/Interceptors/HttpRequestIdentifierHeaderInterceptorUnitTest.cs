namespace Masasamjant.Http.Interceptors
{
    [TestClass]
    public class HttpRequestIdentifierHeaderInterceptorUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new HttpRequestIdentifierHeaderInterceptor(string.Empty));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpRequestIdentifierHeaderInterceptor("  "));
            var interceptor = new HttpRequestIdentifierHeaderInterceptor("Test");
            Assert.AreEqual("Test", interceptor.RequestIdentifierHeaderName);
        }

        [TestMethod]
        public async Task Test_InterceptAsync()
        {
            var getRequest = new HttpGetRequest("api/Test");
            var postRequest = new HttpPostRequest("api/Test", DateTime.Today);
            var interceptor = new HttpRequestIdentifierHeaderInterceptor("Test");
            var getInterception = await interceptor.InterceptAsync(getRequest);
            var postInterception = await interceptor.InterceptAsync(postRequest);
            Assert.IsTrue(getInterception.Result == HttpRequestInterceptionResult.Continue);
            Assert.IsTrue(postInterception.Result == HttpRequestInterceptionResult.Continue);
            var getHeader = getRequest.Headers.Get("Test");
            var postHeader = postRequest.Headers.Get("Test");
            Assert.IsNotNull(getHeader);
            Assert.IsNotNull(postHeader);
            Assert.IsTrue(getHeader.Value == getRequest.Identifier.ToString());
            Assert.IsTrue(postHeader.Value == postRequest.Identifier.ToString());
        }
    }
}
