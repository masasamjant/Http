namespace Masasamjant.Http
{
    [TestClass]
    public class HttpParameterUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new HttpParameter(string.Empty, null));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpParameter("  ", null));
            var parameter = new HttpParameter("name", null);
            Assert.AreEqual("name", parameter.Name);
            Assert.IsNull(parameter.Value);
            parameter = new HttpParameter("name", "value");
            Assert.AreEqual("value", parameter.Value); 
        }

        [TestMethod]
        public void Test_Equals() 
        {
            var parameter = new HttpParameter("name", "value");
            var other = new HttpParameter("name", null);
            Assert.IsTrue(parameter.Equals(other));
            Assert.IsTrue(parameter.GetHashCode() == other.GetHashCode());
            other = new HttpParameter("test", "value");
            Assert.IsFalse(parameter.Equals(other));
            Assert.IsFalse(parameter.Equals(null));
            Assert.IsFalse(parameter.Equals(DateTime.Now));
        }

        [TestMethod]
        public void Test_Clone()
        {
            var parameter = new HttpParameter("name", "value");
            var clone = parameter.Clone();
            Assert.AreEqual(parameter, clone);
            Assert.AreEqual(parameter.Value, clone.Value);
            object copy = ((ICloneable)parameter).Clone();
            Assert.IsInstanceOfType<HttpParameter>(copy);
            Assert.IsFalse(ReferenceEquals(parameter, copy));
            clone = (HttpParameter)copy;
            Assert.AreEqual(parameter, clone);
            Assert.AreEqual(parameter.Value, clone.Value);
        }

        [TestMethod]
        public void Test_ToString()
        {
            Assert.AreEqual("name=", new HttpParameter("name", null).ToString());
            Assert.AreEqual("name=test", new HttpParameter("name", "test").ToString());
            Assert.AreEqual("name=1", new HttpParameter("name", 1).ToString());
        }
    }
}
