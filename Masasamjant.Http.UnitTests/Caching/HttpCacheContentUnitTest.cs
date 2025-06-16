namespace Masasamjant.Http.Caching
{
    [TestClass]
    public class HttpCacheContentUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var content = new HttpCacheContent();
            Assert.AreEqual(string.Empty, content.ContentKey);
            Assert.IsNull(content.ContentValue);
            Assert.IsNull(content.ContentType);

            string contentKey = "key";
            string? contentValue = "value";
            string? contentType = "type";

            content = new HttpCacheContent(contentKey, contentValue, contentType);
            Assert.AreEqual(contentKey, content.ContentKey);
            Assert.AreEqual(contentValue, content.ContentValue);
            Assert.AreEqual(contentType, content.ContentType);
        }
    }
}
