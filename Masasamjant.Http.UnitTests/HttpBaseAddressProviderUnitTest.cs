namespace Masasamjant.Http
{
    [TestClass]
    public class HttpBaseAddressProviderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });

            Assert.ThrowsException<ArgumentNullException>(() => new HttpBaseAddressProvider(configuration, string.Empty, ["Section"]));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpBaseAddressProvider(configuration, "  ", ["Section"]));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpBaseAddressProvider(configuration, "Key", string.Empty));
            Assert.ThrowsException<ArgumentNullException>(() => new HttpBaseAddressProvider(configuration, "Key", "  "));
            var provider = new HttpBaseAddressProvider(configuration, "Key", ["Section"]);
            Assert.IsNotNull(provider);
        }

        [TestMethod]
        public void Test_GetHttpBaseAdress()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "" },
            });
            Assert.ThrowsException<InvalidOperationException>(() => {
                var provider = new HttpBaseAddressProvider(configuration, "Key", ["Section"]);
                provider.GetHttpBaseAdress();
            });

            configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });
            var provider = new HttpBaseAddressProvider(configuration, "Key", ["Section"]);
            string expected = "Value";
            string actual = provider.GetHttpBaseAdress();
            Assert.AreEqual(expected, actual);
        }
    }
}
