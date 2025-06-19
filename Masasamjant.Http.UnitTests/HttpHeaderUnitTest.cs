namespace Masasamjant.Http
{
    [TestClass]
    public class HttpHeaderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new HttpHeader(string.Empty, null));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpHeader("    ", null));
            var header = new HttpHeader("Name", null);
            Assert.AreEqual("Name", header.Name);
            Assert.IsNull(header.Value);
            header = new HttpHeader("Name", "Value");
            Assert.AreEqual("Name", header.Name);
            Assert.AreEqual("Value", header.Value);
        }

        [TestMethod]
        public void Test_Equals()
        {
            var header = new HttpHeader("Name", "Value");
            var other = new HttpHeader("Name", "Test");
            Assert.IsTrue(header.Equals(other));
            Assert.AreEqual(header.GetHashCode(), other.GetHashCode());
            other = new HttpHeader("Test", "Name");
            Assert.IsFalse(header.Equals(other));
        }

        [TestMethod]
        public void Test_Clone()
        {
            var header = new HttpHeader("Name", "Value");
            var clone = header.Clone();
            Assert.IsFalse(ReferenceEquals(header, clone));
            Assert.IsTrue(header.Equals(clone));
            Assert.IsTrue(header.Value == clone.Value);
        }
    }
}
