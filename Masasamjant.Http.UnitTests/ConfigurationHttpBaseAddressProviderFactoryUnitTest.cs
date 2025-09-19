namespace Masasamjant.Http
{
    [TestClass]
    public class ConfigurationHttpBaseAddressProviderFactoryUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });
            var factory = new ConfigurationHttpBaseAddressProviderFactory(configuration);
            Assert.IsNotNull(factory);
            Assert.ThrowsException<ArgumentNullException>(() => new ConfigurationHttpBaseAddressProviderFactory(configuration, string.Empty));
            Assert.ThrowsException<ArgumentNullException>(() => new ConfigurationHttpBaseAddressProviderFactory(configuration, "  "));
            Assert.ThrowsException<ArgumentException>(() => new ConfigurationHttpBaseAddressProviderFactory(configuration, "Test"));
            factory = new ConfigurationHttpBaseAddressProviderFactory(configuration, "Section");
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void Test_ConfigurationKey()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });
            var factory = new ConfigurationHttpBaseAddressProviderFactory(configuration);
            Assert.AreEqual("HttpBaseAddress", factory.ConfigurationKey);
            factory.ConfigurationKey = "Key";
            Assert.AreEqual("Key", factory.ConfigurationKey);
        }

        [TestMethod]
        public void Test_GetBaseAddressProvider() 
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });
            var factory = new ConfigurationHttpBaseAddressProviderFactory(configuration);
            factory.ConfigurationKey = "Key";
            Assert.ThrowsException<ArgumentNullException>(() => factory.GetBaseAddressProvider(string.Empty));
            Assert.ThrowsException<ArgumentNullException>(() => factory.GetBaseAddressProvider("    "));
            var provider = factory.GetBaseAddressProvider("Section");
            var value = provider.GetHttpBaseAdress();
            Assert.AreEqual("Value", value);
        }
    }
}
