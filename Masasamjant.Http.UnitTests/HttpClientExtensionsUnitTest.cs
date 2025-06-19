using Masasamjant.Http.Interceptors;

namespace Masasamjant.Http
{
    [TestClass]
    public class HttpClientExtensionsUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_AddRequestIdentifierHeaderInterceptor()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddRequestIdentifierHeaderInterceptor(client, "Test");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as HttpRequestIdentifierHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
            Assert.IsTrue(headerInterceptor.RequestIdentifierHeaderName == "Test");
        }
    }
}
