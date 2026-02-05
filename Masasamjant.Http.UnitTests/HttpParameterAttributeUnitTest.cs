namespace Masasamjant.Http
{
    [TestClass]
    public class HttpParameterAttributeUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var attribute = new HttpParameterAttribute("Name");
            Assert.AreEqual("Name", attribute.ParameterName);
        }

        [TestMethod]
        public void Test_Constructor_Validate_Name()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new HttpParameterAttribute(""));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpParameterAttribute("  "));
        }
    }
}
