namespace Masasamjant.Http.Abstractions
{
    [TestClass]
    public class HttpCacheManagerUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_GetContentKey()
        {
            var manager = new TestHttpCacheManager();
            HttpGetRequest request1 = new HttpGetRequest("api/Test1", [HttpParameter.From("name", "value")]);
            HttpGetRequest request2 = new HttpGetRequest("api/Test1", [HttpParameter.From("name", "value")]);
            var contentKey1 = manager.TestGetContentKey(request1);
            var contentKey2 = manager.TestGetContentKey(request2);
            Assert.AreEqual(contentKey1, contentKey2);
            request1 = new HttpGetRequest("api/Test1", [HttpParameter.From("name", "1")]);
            request2 = new HttpGetRequest("api/Test1", [HttpParameter.From("name", "2")]);
            contentKey1 = manager.TestGetContentKey(request1);
            contentKey2 = manager.TestGetContentKey(request2);
            Assert.AreNotEqual(contentKey1, contentKey2);
            request1 = new HttpGetRequest("api/Test1", [HttpParameter.From("name", "value")]);
            request2 = new HttpGetRequest("api/Test2", [HttpParameter.From("name", "value")]);
            contentKey1 = manager.TestGetContentKey(request1);
            contentKey2 = manager.TestGetContentKey(request2);
            Assert.AreNotEqual(contentKey1, contentKey2);
        }

        [TestMethod]
        public async Task Test_Default()
        {
            var manager = HttpCacheManager.Default;
            var request = new HttpGetRequest("api/Test", [HttpParameter.From("name", "value")]);
            await manager.AddCacheContentAsync(request, "content", "string", TimeSpan.FromSeconds(10));
            var content = await manager.GetCacheContentAsync(request);
            Assert.IsNull(content);
        }

        [TestMethod]
        public async Task Test_RemoveCacheContentAsync()
        {
            var manager = HttpCacheManager.Default;
            var request = new HttpGetRequest("api/Test", [HttpParameter.From("name", "value")]);
            await manager.RemoveCacheContentAsync(request);
        }
    }
}
