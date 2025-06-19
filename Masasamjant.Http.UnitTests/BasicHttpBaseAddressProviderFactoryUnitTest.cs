namespace Masasamjant.Http
{
    [TestClass]
    public class BasicHttpBaseAddressProviderFactoryUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var httpBaseAddresses = new Dictionary<string, string>();
            Assert.ThrowsException<ArgumentException>(() => new BasicHttpBaseAddressProviderFactory(httpBaseAddresses));
            httpBaseAddresses.Add("Test", "Test");
            var factory = new BasicHttpBaseAddressProviderFactory(httpBaseAddresses);
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void Test_GetBaseAddressProvider()
        {
            var httpBaseAddresses = new Dictionary<string, string>()
            {
                { "1", "1" },
                { "2", "2" },
                { "3", "" },
            };         
            var factory = new BasicHttpBaseAddressProviderFactory(httpBaseAddresses);
            Assert.ThrowsException<ArgumentNullException>(() => factory.GetBaseAddressProvider(string.Empty));
            Assert.ThrowsException<ArgumentNullException>(() => factory.GetBaseAddressProvider("    "));
            Assert.ThrowsException<ArgumentException>(() => factory.GetBaseAddressProvider("3"));
            Assert.ThrowsException<ArgumentException>(() => factory.GetBaseAddressProvider("4"));
            Assert.AreEqual("1", factory.GetBaseAddressProvider("1").GetHttpBaseAdress());
            Assert.AreEqual("2", factory.GetBaseAddressProvider("2").GetHttpBaseAdress());
        }
    }
}
