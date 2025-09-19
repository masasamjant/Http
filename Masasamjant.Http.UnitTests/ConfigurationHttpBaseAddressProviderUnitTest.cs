namespace Masasamjant.Http
{
    [TestClass]
    public class ConfigurationHttpBaseAddressProviderUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });

            Assert.ThrowsException<ArgumentNullException>(() => new ConfigurationHttpBaseAddressProvider(configuration, string.Empty, ["Section"]));
            Assert.ThrowsException<ArgumentNullException>(() => new ConfigurationHttpBaseAddressProvider(configuration, "  ", ["Section"]));
            Assert.ThrowsException<ArgumentNullException>(() => new ConfigurationHttpBaseAddressProvider(configuration, "Key", string.Empty));
            Assert.ThrowsException<ArgumentNullException>(() => new ConfigurationHttpBaseAddressProvider(configuration, "Key", "  "));
            var provider = new ConfigurationHttpBaseAddressProvider(configuration, "Key", ["Section"]);
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
                var provider = new ConfigurationHttpBaseAddressProvider(configuration, "Key", ["Section"]);
                provider.GetHttpBaseAdress();
            });

            configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });
            var provider = new ConfigurationHttpBaseAddressProvider(configuration, "Key", ["Section"]);
            string expected = "Value";
            string actual = provider.GetHttpBaseAdress();
            Assert.AreEqual(expected, actual);
        }
    }
}
