namespace Masasamjant.Http
{
    [TestClass]
    public class HttpHeaderCollectionUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var headers = new HttpHeaderCollection();
            Assert.IsTrue(headers.Count == 0);
        }

        [TestMethod]
        public void Test_Add()
        {
            var headers = new HttpHeaderCollection();
            var header = new HttpHeader("name", "1");
            headers.Add(header);
            Assert.IsTrue(headers.Count == 1);
            Assert.IsTrue(headers.Contains(header));
            Assert.ThrowsException<ArgumentException>(() => headers.Add(new HttpHeader("name", "2")));
            header = headers.Add("test", "2");
            Assert.IsTrue(header.Value == "2");
            Assert.IsTrue(header.Name == "test");
            Assert.IsTrue(headers.Count == 2);
        }

        [TestMethod]
        public void Test_Clear()
        {
            var headers = new HttpHeaderCollection();
            headers.Add("1", "1");
            headers.Add("2", "2");
            Assert.IsTrue(headers.Count == 2);
            headers.Clear();
            Assert.IsTrue(headers.Count == 0);
        }

        [TestMethod]
        public void Test_Contains()
        {
            var headers = new HttpHeaderCollection();
            var header = new HttpHeader("name", "1");
            headers.Add(header);
            Assert.IsTrue(headers.Contains(header));
            Assert.IsTrue(headers.Contains("name"));
            Assert.IsFalse(headers.Contains(new HttpHeader("test", "1")));
            Assert.IsFalse(headers.Contains("test"));
        }

        [TestMethod]
        public void Test_Get()
        {
            var headers = new HttpHeaderCollection();
            var header = headers.Get("name");
            Assert.IsNull(header);
            headers.Add("name", "test");
            header = headers.Get("name");
            Assert.IsNotNull(header);
        }

        [TestMethod]
        public void Test_Remove()
        {
            var headers = new HttpHeaderCollection();
            Assert.IsFalse(headers.Remove("name"));
            var header = new HttpHeader("name", "test");
            headers.Add(header);
            Assert.IsTrue(headers.Remove("name"));
            Assert.IsFalse(headers.Remove(header));
            headers.Add("name", "foo");
            Assert.IsTrue(headers.Remove(header));
        }
    }
}
