namespace Masasamjant.Http
{
    [TestClass]
    public class HttpRequestKeyUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var parameters = new[] { HttpParameter.From("1", "1") };
            var request = new HttpGetRequest("test/Api", parameters);
            var key = new HttpRequestKey(request);
            Assert.AreEqual(request.GetFullRequestUri(), key.RequestUri);
            Assert.AreEqual(request.Identifier, key.RequestIdentifier);
            Assert.AreEqual(request.Method, key.RequestMethod);
        }

        [TestMethod]
        public void Test_Equality()
        {
            var parameters = new[] { HttpParameter.From("1", "1") };
            var request = new HttpGetRequest("test/Api", parameters);
            var key = new HttpRequestKey(request);
            var other = new HttpRequestKey(request);
            Assert.IsTrue(key.Equals(other));
            Assert.AreEqual(key.GetHashCode(), other.GetHashCode());
            request.Parameters.Add("2", "2");
            other = new HttpRequestKey(request);
            Assert.IsFalse(key.Equals(other));
            Assert.IsFalse(key.Equals(null));
            Assert.IsFalse(key.Equals(DateTime.Now));
            other = new HttpRequestKey(new HttpPostRequest("test/Api", 1));
            Assert.IsFalse(key.Equals(other));
            other = new HttpRequestKey(new HttpGetRequest("test/Api", parameters));
            Assert.IsFalse(key.Equals(other));
        }
    }
}
