namespace Masasamjant.Http.Interceptors
{
    [TestClass]
    public class HttpGetRequestInterceptorCollectionUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var collection = new HttpGetRequestInterceptorCollection();
            Assert.IsTrue(collection.Count == 0);
        }

        [TestMethod]
        public void Test_Add()
        {
            var lines = new List<string>();
            var interceptor = new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines);
            var collection = new HttpGetRequestInterceptorCollection();
            collection.Add(interceptor);
            collection.Add(interceptor);
            Assert.IsTrue(collection.Count == 1);
            Assert.IsTrue(collection.Contains(interceptor));
        }

        [TestMethod]
        public void Test_Clear()
        {
            var lines = new List<string>();
            var interceptor = new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines);
            var collection = new HttpGetRequestInterceptorCollection();
            collection.Add(interceptor);
            Assert.IsTrue(collection.Count == 1);
            collection.Clear();
            Assert.IsTrue(collection.Count == 0);
        }

        [TestMethod]
        public void Test_Contains()
        {
            var lines = new List<string>();
            var interceptor = new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines);
            var collection = new HttpGetRequestInterceptorCollection();
            Assert.IsFalse(collection.Contains(interceptor));
            collection.Add(interceptor);
            Assert.IsTrue(collection.Contains(interceptor));
        }

        [TestMethod]
        public void Test_Remove()
        {
            var lines = new List<string>();
            var interceptor = new TestHttpRequestInterceptor(HttpRequestInterception.Continue, lines);
            var collection = new HttpGetRequestInterceptorCollection();
            Assert.IsFalse(collection.Remove(interceptor));
            collection.Add(interceptor);
            Assert.IsTrue(collection.Remove(interceptor));
        }
    }
}
