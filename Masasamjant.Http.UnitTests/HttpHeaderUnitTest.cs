namespace Masasamjant.Http
{
    [TestClass]
    public class HttpHeaderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var header = new HttpHeader("Name", null);
            Assert.AreEqual("Name", header.Name);
            Assert.IsNull(header.Value);
            header = new HttpHeader("Name", "Value");
            Assert.AreEqual("Name", header.Name);
            Assert.AreEqual("Value", header.Value);
        }

        [TestMethod]
        public void Test_Constructor_Validate_Name()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new HttpHeader(string.Empty, null));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpHeader("    ", null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new HttpHeader(new string('A', 41), null));
            Assert.ThrowsException<ArgumentException>(() => new HttpHeader("Pää", null));
        }

        [TestMethod]
        public void Test_Constructor_Validate_Value()
        {
            Assert.ThrowsException<ArgumentException>(() => new HttpHeader("Name", "Pää"));
            Assert.ThrowsException<ArgumentException>(() => new HttpHeader("Name", "½"));
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
            Assert.IsFalse(header.Equals(null));
            Assert.IsFalse(header.Equals(DateTime.Now));
        }

        [TestMethod]
        public void Test_Clone()
        {
            var header = new HttpHeader("Name", "Value");
            var clone = header.Clone();
            Assert.IsFalse(ReferenceEquals(header, clone));
            Assert.IsTrue(header.Equals(clone));
            Assert.IsTrue(header.Value == clone.Value);

            object copy = ((ICloneable)header).Clone();
            Assert.IsInstanceOfType<HttpHeader>(copy);
            Assert.IsFalse(ReferenceEquals(header, copy));
            clone = (HttpHeader)copy;
            Assert.IsTrue(header.Equals(clone));
            Assert.IsTrue(header.Value == clone.Value);
        }
    }
}
