using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

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

            provider = new ConfigurationHttpBaseAddressProvider(configuration, "Key");
            Assert.IsNotNull(provider);
        }

        [TestMethod]
        public void Test_GetHttpBaseAdress()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Section:Key", "Value" },
            });
            var provider = new ConfigurationHttpBaseAddressProvider(configuration, "Key", ["Section"]);
            string expected = "Value";
            string actual = provider.GetHttpBaseAdress();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetHttpBaseAdress_NoSection()
        {
            var configuration = GetConfiguration(new Dictionary<string, string?>()
            {
                { "Key", "" },
            });
            Assert.ThrowsException<InvalidOperationException>(() => {
                var provider = new ConfigurationHttpBaseAddressProvider(configuration, "Key");
                provider.GetHttpBaseAdress();
            });
        }

        [TestMethod]
        public void Test_GetHttpBaseAddress_Configuration_Exception()
        {
            var configuration = new TestConfiguration();
            Assert.ThrowsException<InvalidOperationException>(() => {
                var provider = new ConfigurationHttpBaseAddressProvider(configuration, "Key");
                provider.GetHttpBaseAdress();
            });
        }

        private class TestConfiguration : IConfiguration
        {
            public string? this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public IEnumerable<IConfigurationSection> GetChildren()
            {
                throw new NotImplementedException();
            }

            public IChangeToken GetReloadToken()
            {
                throw new NotImplementedException();
            }

            public IConfigurationSection GetSection(string key)
            {
                throw new NotImplementedException();
            }
        }
    }
}
