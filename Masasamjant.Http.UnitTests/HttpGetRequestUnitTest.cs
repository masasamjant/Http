using Masasamjant.Http.Caching;

namespace Masasamjant.Http
{
    [TestClass]
    public class HttpGetRequestUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var request = new HttpGetRequest("api/Test");
            Assert.IsNotNull(request);
            Assert.IsNotNull(request.Caching);
            Assert.IsFalse(request.Caching.CanCacheResult);
            Assert.AreEqual("api/Test", request.FullRequestUri);
            Assert.AreEqual(request.FullRequestUri, request.GetFullRequestUri());
            var parameters = new[] 
            {
                HttpParameter.From("1", "1"),
                HttpParameter.From("2", "2"),
            };
            request = new HttpGetRequest("api/Test", parameters, new HttpGetRequestCaching(true, TimeSpan.FromMinutes(5)));
            Assert.IsTrue(request.Parameters.Count == 2 && request.Parameters.Contains("1") && request.Parameters.Contains("2"));
            string expected = "api/Test?1=1&2=2";
            string actual = request.FullRequestUri;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(request.FullRequestUri, request.GetFullRequestUri());
            Assert.AreEqual("?1=1&2=2", request.QueryString);
            Assert.IsNotNull(request.Caching);
            Assert.IsTrue(request.Caching.CanCacheResult);
            Assert.IsFalse(request.Caching.IsCacheResult);
        }

        [TestMethod]
        public void Test_Clone()
        {
            var parameters = new[]
            {
                HttpParameter.From("1", "1"),
                HttpParameter.From("2", "2"),
            };
            
            var request = new HttpGetRequest("api/Test", parameters, new HttpGetRequestCaching(true, TimeSpan.FromMinutes(5)));
            request.Headers.Add("1", "1");

            var clone = request.Clone();
            Assert.IsFalse(ReferenceEquals(request, clone));
            Assert.AreEqual(request.GetFullRequestUri(), clone.FullRequestUri); 
            
            Assert.AreEqual(request.Headers.Count, clone.Headers.Count);
            foreach (var header in request.Headers)
                Assert.IsTrue(clone.Headers.Contains(header) && clone.Headers.Get(header.Name)!.Value == header.Value);

            Assert.AreEqual(request.Parameters.Count, clone.Parameters.Count);
            foreach (var parameter in request.Parameters)
                Assert.IsTrue(clone.Parameters.Contains(parameter) && clone.Parameters.Get(parameter.Name)!.Value == parameter.Value);

            Assert.IsFalse(ReferenceEquals(request.Caching, clone.Caching));
            Assert.AreEqual(request.Caching.CanCacheResult, clone.Caching.CanCacheResult);
            Assert.AreEqual(request.Caching.CacheDuration, clone.Caching.CacheDuration);

            object copy = ((ICloneable)request).Clone();
            Assert.IsInstanceOfType<HttpGetRequest>(copy);
            Assert.IsFalse(ReferenceEquals(request, copy));
            HttpGetRequest other = (HttpGetRequest)copy;
            Assert.AreEqual(request.GetFullRequestUri(), other.FullRequestUri);
            Assert.AreEqual(request.Headers.Count, other.Headers.Count);
            foreach (var header in request.Headers)
                Assert.IsTrue(other.Headers.Contains(header) && other.Headers.Get(header.Name)!.Value == header.Value);
            Assert.AreEqual(request.Parameters.Count, other.Parameters.Count);
            foreach (var parameter in request.Parameters)
                Assert.IsTrue(other.Parameters.Contains(parameter) && other.Parameters.Get(parameter.Name)!.Value == parameter.Value);
            Assert.IsFalse(ReferenceEquals(request.Caching, other.Caching));
            Assert.AreEqual(request.Caching.CanCacheResult, other.Caching.CanCacheResult);
            Assert.AreEqual(request.Caching.CacheDuration, other.Caching.CacheDuration);
        }
    }
}
