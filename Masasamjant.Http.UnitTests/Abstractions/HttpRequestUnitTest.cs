namespace Masasamjant.Http.Abstractions
{
    [TestClass]
    public class HttpRequestUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TestHttpRequest(string.Empty, HttpRequestMethod.Put));
            Assert.ThrowsException<ArgumentNullException>(() => new TestHttpRequest("   ", HttpRequestMethod.Put));
            Assert.ThrowsException<ArgumentException>(() => new TestHttpRequest("api/Test", (HttpRequestMethod)999));
            HttpRequest request = new TestHttpRequest("api/Test", HttpRequestMethod.Put);
            Assert.AreEqual("api/Test", request.RequestUri);
            Assert.AreEqual(HttpRequestMethod.Put, request.Method);
            Assert.IsTrue(request.Headers.Count == 0);
        }

        [TestMethod]
        public void Test_Cancel()
        {
            bool canceled = false;
            HttpRequest request = new TestHttpRequest("api/Test", HttpRequestMethod.Put);
            request.Canceled += (s, e) => canceled = true;
            request.Cancel();
            Assert.IsTrue(canceled);
        }

        [TestMethod]
        public void Test_GetRequestKey()
        {
            HttpRequest request = new TestHttpRequest("api/Test", HttpRequestMethod.Put);
            var expected = new HttpRequestKey(request);
            var actual = request.GetHttpRequestKey();
            Assert.IsFalse(ReferenceEquals(expected, actual));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_CloneHeadersTo()
        {
            var request = new TestHttpRequest("api/Test", HttpRequestMethod.Put);
            var header = request.Headers.Add("name", "test");
            var other = new TestHttpRequest("api/Other", HttpRequestMethod.Delete);
            request.TestCloneHeadersTo(other);
            var otherHeader = other.Headers.Get("name");
            Assert.IsFalse(ReferenceEquals(otherHeader, header));
            Assert.IsTrue(header.Equals(otherHeader));
        }
    }
}
