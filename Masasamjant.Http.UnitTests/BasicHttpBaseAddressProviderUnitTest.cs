namespace Masasamjant.Http
{
    [TestClass]
    public class BasicHttpBaseAddressProviderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Provider()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BasicHttpBaseAddressProvider(string.Empty));
            Assert.ThrowsException<ArgumentNullException>(() => new BasicHttpBaseAddressProvider("   "));
            var provider = new BasicHttpBaseAddressProvider(" Test ");
            Assert.IsNotNull(provider);
            string expected = " Test ";
            string actual = provider.GetHttpBaseAdress();
            Assert.AreEqual(expected, actual);
        }
    }
}
