namespace Masasamjant.Http.Caching
{
    [TestClass]
    public class MemoryHttpCacheManagerUnitTest : UnitTest
    {
        [TestMethod]
        public async Task Test_AddCacheContentAsync_GetCacheContentAsync()
        {
            var caching = new HttpGetRequestCaching(true, TimeSpan.FromMilliseconds(100));
            var manager = new MemoryHttpCacheManager();
            var request1 = new HttpGetRequest("Testing/Request/1", caching);
            await manager.AddCacheContentAsync(request1, "Testing", "string", TimeSpan.FromMilliseconds(100));
            var content = await manager.GetCacheContentAsync(request1);
            Assert.IsNotNull(content);
            Assert.AreEqual("Testing", content.ContentValue);
            Assert.AreEqual("string", content.ContentType);
            await manager.AddCacheContentAsync(request1, "1", "int", TimeSpan.FromMilliseconds(100));
            content = await manager.GetCacheContentAsync(request1);
            Assert.IsNotNull(content);
            Assert.AreEqual("1", content.ContentValue);
            Assert.AreEqual("int", content.ContentType);
            var request2 = new HttpGetRequest("Testing/Request/2", caching);
            content = await manager.GetCacheContentAsync(request2);
            Assert.IsNull(content);
            Thread.Sleep(100);
            content = await manager.GetCacheContentAsync(request1);
            Assert.IsNull(content);
        }

        [TestMethod]
        public async Task Test_GetCacheContentAsync_Not_Content()
        {
            var caching = new HttpGetRequestCaching(true, TimeSpan.FromMilliseconds(100));
            var manager = new MemoryHttpCacheManager();
            var request1 = new HttpGetRequest("Testing/Request/1", caching);
            var content = await manager.GetCacheContentAsync(request1);
            Assert.IsNull(content);
        }
    }
}
