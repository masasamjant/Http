using Masasamjant.Configuration;
using Masasamjant.Http.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents provider of HTTP base address that reads HTTP base address from configuration.
    /// </summary>
    public class HttpBaseAddressProvider : IHttpBaseAddressProvider
    {
        private readonly IConfiguration configuration;
        private readonly string configurationKey;
        private readonly IEnumerable<string> sectionKeys;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpBaseAddressProvider"/> class. This constructor 
        /// should be used when configuration value is read from <see cref="IConfiguration"/> and not some sub-section.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="configurationKey">The configuration key.</param>
        public HttpBaseAddressProvider(IConfiguration configuration, string configurationKey)
            : this(configuration, configurationKey, [])
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="HttpBaseAddressProvider"/> class. This constructor 
        /// should be used when configuration value is read from configuration section of <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="configurationKey">The configuration key.</param>
        /// <param name="sectionKey">The configuration section key.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="configurationKey"/> or <paramref name="sectionKey"/> is empty or contains only whitespace characters.</exception>
        public HttpBaseAddressProvider(IConfiguration configuration, string configurationKey, string sectionKey)
            : this(configuration, configurationKey, [sectionKey])
        {
            if (string.IsNullOrWhiteSpace(sectionKey))
                throw new ArgumentNullException(nameof(sectionKey), "The section key is empty or contains only whitespace characters.");
        }

        /// <summary>
        /// Initializes new instance of the <see cref="HttpBaseAddressProvider"/> class. This constuctor 
        /// should be used when configuration value is read from deeper configuration section of <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="configurationKey">The configuration key.</param>
        /// <param name="sectionKeys">The configuration section keys or empty.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="configurationKey"/> is empty or contains only whitespace characters.</exception>
        public HttpBaseAddressProvider(IConfiguration configuration, string configurationKey, IEnumerable<string> sectionKeys)
        {
            if (string.IsNullOrWhiteSpace(configurationKey))
                throw new ArgumentNullException(nameof(configurationKey), "The configuration key is empty or contains only whitespace characters.");

            this.configuration = configuration;
            this.configurationKey = configurationKey;
            this.sectionKeys = sectionKeys;
        }

        /// <summary>
        /// Gets the HTTP base address.
        /// </summary>
        /// <returns>A HTTP base address.</returns>
        /// <exception cref="InvalidOperationException">If could not get HTTP base address.</exception>
        public string GetHttpBaseAdress()
        {
            try
            {
                var value = configuration.GetValue(configurationKey, sectionKeys);

                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("The configuration value is null, empty or contains only whitespace characters.");

                return value;
            }
            catch (ConfigurationException exception)
            {
                throw new InvalidOperationException("The configuration contains errors.", exception);
            }
        }
    }
}
