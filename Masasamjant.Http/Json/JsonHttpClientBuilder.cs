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
        /// Creates instance of <see cref="JsonHttpClient"/> class.
        /// </summary>
        /// <param name="clientPurpose"></param>
        /// <returns></returns>
        protected override IHttpClient CreateClient(string clientPurpose)
        {
            var baseAddressProvider = HttpBaseAddressProviderFactory.GetBaseAddressProvider(clientPurpose);
            var client = new JsonHttpClient(HttpClientFactory, baseAddressProvider);
            return client;
        }
    }
}
