using Masasamjant.Http.Abstractions;
using Masasamjant.Xml;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http.Xml
{
    /// <summary>
    /// Represents component to build instance of <see cref="XmlHttpClient"/> class.
    /// </summary>
    public sealed class XmlHttpClientBuilder : HttpClientBuilder
    {
        /// <summary>
        /// Initializes new instance of the <see cref="XmlHttpClientBuilder"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="httpBaseAddressProviderFactory">The <see cref="IHttpBaseAddressProviderFactory"/>.</param>
        /// <param name="cacheManager">The <see cref="IHttpCacheManager"/>.</param>
        public XmlHttpClientBuilder(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpBaseAddressProviderFactory httpBaseAddressProviderFactory, IHttpCacheManager? cacheManager)
            : base(configuration, httpClientFactory, httpBaseAddressProviderFactory, cacheManager)
        { }

        /// <summary>
        /// Creates instance of <see cref="XmlHttpClient"/> class.
        /// </summary>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        /// <returns>A <see cref="XmlHttpClient"/>.</returns>
        protected override IHttpClient CreateClient(string clientPurpose)
        {
            var baseAddressProvider = HttpBaseAddressProviderFactory.GetBaseAddressProvider(clientPurpose);
            var client = new XmlHttpClient(HttpClientFactory, baseAddressProvider, CacheManager);
            return client;
        }

        /// <summary>
        /// Configure the created <see cref="IHttpClient"/> instance before it is used. This checks for XML serialization in configuration and if 
        /// override found and valid, then sets configured value.
        /// </summary>
        /// <param name="client">The <see cref="IHttpClient"/> obtained from <see cref="CreateClient(string)"/>.</param>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        protected override void ConfigureClient(IHttpClient client, string clientPurpose)
        {
            var httpClientSection = Configuration.GetRequiredSection("HttpClient");
            var clientPurposeSection = Configuration.GetRequiredSection(clientPurpose);
            var serialization = clientPurposeSection["XmlSerialization"];
            
            if (!string.IsNullOrWhiteSpace(serialization) && Enum.TryParse<XmlSerialization>(serialization, true, out var result))
            {
                ((XmlHttpClient)client).XmlSerialization = result;
            }
        }
    }
}
