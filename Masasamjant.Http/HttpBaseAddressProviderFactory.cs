using Masasamjant.Http.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents <see cref="IHttpBaseAddressProviderFactory"/> that use configuration to create <see cref="HttpBaseAddressProvider"/> instances.
    /// </summary>
    public class HttpBaseAddressProviderFactory : IHttpBaseAddressProviderFactory
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpBaseAddressProviderFactory"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="rootSectionKey">The root configuration section key.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="rootSectionKey"/> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ArgumentException">If configuration section specified by <paramref name="rootSectionKey"/> does not exist.</exception>
        public HttpBaseAddressProviderFactory(IConfiguration configuration, string rootSectionKey)
        {
            if (string.IsNullOrWhiteSpace(rootSectionKey))
                throw new ArgumentNullException(nameof(rootSectionKey), "The root section key is empty or contains only whitespace characters.");

            try
            {
                this.configuration = configuration.GetRequiredSection(rootSectionKey);
            }
            catch (InvalidOperationException exception)
            {
                throw new ArgumentException("The configuration section does not exist.", nameof(rootSectionKey), exception);
            }
        }

        /// <summary>
        /// Initializes new instance of the <see cref="HttpBaseAddressProviderFactory"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public HttpBaseAddressProviderFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets or sets the configuration key for actual base address configuration value. 
        /// Default value is "HttpBaseAddress".
        /// </summary>
        public string ConfigurationKey { get; set; } = "HttpBaseAddress";

        /// <summary>
        /// Creates instance of <see cref="HttpBaseAddressProvider"/> for specified base address purpose.
        /// </summary>
        /// <param name="baseAddressPurpose">The purpose of the HTTP base address and name of configuration section of the HTTP base address.</param>
        /// <returns>A <see cref="IHttpBaseAddressProvider"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="baseAddressPurpose"/> is empty or contains only whitespace characters.</exception>
        /// <exception cref="InvalidOperationException">If failed to create instance of <see cref="IHttpBaseAddressProvider"/> implementation.</exception>
        public IHttpBaseAddressProvider GetBaseAddressProvider(string baseAddressPurpose)
        {
            if (string.IsNullOrWhiteSpace(baseAddressPurpose))
                throw new ArgumentNullException(nameof(baseAddressPurpose), "The base address purpose is empty or contains only whitespace characters.");

            try
            {
                return new HttpBaseAddressProvider(configuration, ConfigurationKey, baseAddressPurpose);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not create instance of '{typeof(HttpBaseAddressProvider)}'.", exception);
            }
        }
    }
}
