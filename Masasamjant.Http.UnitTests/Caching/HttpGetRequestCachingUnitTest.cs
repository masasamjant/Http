namespace Masasamjant.Http.Caching
{
    [TestClass]
    public class HttpGetRequestCachingUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var caching = new HttpGetRequestCaching();
            Assert.IsFalse(caching.CanCacheResult);
            Assert.IsFalse(caching.IsCacheResult);
            Assert.AreEqual(TimeSpan.Zero, caching.CacheDuration);

            bool canCacheResult = true;
            TimeSpan cacheDuration = TimeSpan.Zero;
            caching = new HttpGetRequestCaching(canCacheResult, cacheDuration);
            Assert.IsFalse(caching.CanCacheResult);
            Assert.IsFalse(caching.IsCacheResult);
            Assert.AreEqual(TimeSpan.Zero, caching.CacheDuration);

            cacheDuration = TimeSpan.FromMinutes(5);
            caching = new HttpGetRequestCaching(canCacheResult, cacheDuration);
            Assert.IsTrue(caching.CanCacheResult);
            Assert.IsFalse(caching.IsCacheResult);
            Assert.AreEqual(cacheDuration, caching.CacheDuration);
        }
    }
}
