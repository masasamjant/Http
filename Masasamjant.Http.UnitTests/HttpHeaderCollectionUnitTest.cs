using System.Collections;

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

            var other = new HttpHeaderCollection();
            var h1 = other.Add("1", "1");
            headers = new HttpHeaderCollection(other);
            Assert.IsTrue(headers.Count == 1);
            Assert.IsTrue(headers.Contains(h1));
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

        [TestMethod]
        public void Test_IsReadOnly()
        {
            ICollection<HttpHeader> collection = new HttpHeaderCollection();
            Assert.IsFalse(collection.IsReadOnly);
        }

        [TestMethod]
        public void Test_CopyTo()
        {
            var other = new HttpHeaderCollection();
            var header1 = other.Add("Key1", "1");
            var header2 = other.Add("Key2", "2");
            ICollection<HttpHeader> collection = new HttpHeaderCollection(other);
            HttpHeader[] expected = new HttpHeader[] { header1, header2 };
            HttpHeader[] array = new HttpHeader[2];
            collection.CopyTo(array, 0);
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Test_GetEnumerator()
        {
            var collection = new HttpHeaderCollection();
            IEnumerator enumerator1 = ((IEnumerable)collection).GetEnumerator();
            IEnumerator<HttpHeader> enumerator2 = collection.GetEnumerator();
            Assert.AreEqual(enumerator1.GetType(), enumerator2.GetType());
        }
    }
}
