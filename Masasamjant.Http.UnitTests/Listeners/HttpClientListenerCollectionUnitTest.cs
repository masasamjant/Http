using System.Text;

namespace Masasamjant.Http.Listeners
{
    [TestClass]
    public class HttpClientListenerCollectionUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var listeners = new HttpClientListenerCollection();
            Assert.IsTrue(listeners.Count == 0);
        }

        [TestMethod]
        public void Test_Add()
        {
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            var listeners = new HttpClientListenerCollection();
            listeners.Add(listener);
            listeners.Add(listener);
            Assert.IsTrue(listeners.Count == 1);
            Assert.IsTrue(listeners.Contains(listener));
        }

        [TestMethod]
        public void Test_Clear()
        {
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            var listeners = new HttpClientListenerCollection();
            listeners.Add(listener);
            Assert.IsTrue(listeners.Count == 1);
            listeners.Clear();
            Assert.IsTrue(listeners.Count == 0);
        }

        [TestMethod]
        public void Test_Contains()
        {
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            var listeners = new HttpClientListenerCollection();
            Assert.IsFalse(listeners.Contains(listener));   
            listeners.Add(listener);
            Assert.IsTrue(listeners.Contains(listener));
        }

        [TestMethod]
        public void Test_Remove()
        {
            var builder = new StringBuilder();
            var listener = new TestHttpClientListener(builder);
            var listeners = new HttpClientListenerCollection();
            Assert.IsFalse(listeners.Remove(listener));
            listeners.Add(listener);
            Assert.IsTrue(listeners.Remove(listener));
        }
    }
}
