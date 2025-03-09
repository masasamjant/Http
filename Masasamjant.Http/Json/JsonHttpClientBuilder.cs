using Masasamjant.Http.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http.Json
{
    /// <summary>
    /// Represents component to build instance of <see cref="JsonHttpClient"/> class.
    /// </summary>
    public class JsonHttpClientBuilder : HttpClientBuilder
    {
        /// <summary>
        /// Initializes new instance of the <see cref="JsonHttpClientBuilder"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="httpBaseAddressProviderFactory">The <see cref="IHttpBaseAddressProviderFactory"/>.</param>
        public JsonHttpClientBuilder(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpBaseAddressProviderFactory httpBaseAddressProviderFactory) 
            : base(configuration, httpClientFactory, httpBaseAddressProviderFactory)
        {
        }

        /// <summary>
        /// Builds instance of <see cref="JsonHttpClient"/> for specified purpose.
        /// </summary>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        /// <returns>A <see cref="JsonHttpClient"/>.</returns>
        public override IHttpClient Build(string clientPurpose)
        {
            var baseAddressProvider = HttpBaseAddressProviderFactory.GetBaseAddressProvider(clientPurpose);
            var client = new JsonHttpClient(HttpClientFactory, baseAddressProvider);
            ConfigureClient(client);
            return client;
        }

        /// <summary>
        /// Configures instance of <see cref="JsonHttpClient"/> before it is used.
        /// </summary>
        /// <param name="client">The <see cref="JsonHttpClient"/> to configure.</param>
        protected virtual void ConfigureClient(JsonHttpClient client)
        {
            return;
        }
    }
}
