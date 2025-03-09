using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents abstract component to build instance of <see cref="IHttpClient"/> implementation.
    /// </summary>
    public abstract class HttpClientBuilder : IHttpClientBuilder
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpClientBuilder"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="httpBaseAddressProviderFactory">The <see cref="IHttpBaseAddressProviderFactory"/>.</param>
        protected HttpClientBuilder(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpBaseAddressProviderFactory httpBaseAddressProviderFactory)
        {
            Configuration = configuration;
            HttpClientFactory = httpClientFactory;
            HttpBaseAddressProviderFactory = httpBaseAddressProviderFactory;
        }

        /// <summary>
        /// Gets the <see cref="IHttpClientFactory"/>.
        /// </summary>
        protected IHttpClientFactory HttpClientFactory { get; }

        /// <summary>
        /// Gets the <see cref="IHttpBaseAddressProviderFactory"/>.
        /// </summary>
        protected IHttpBaseAddressProviderFactory HttpBaseAddressProviderFactory { get; }

        /// <summary>
        /// Gets the <see cref="IConfiguration"/>.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Builds instance of <see cref="IHttpClient"/> implementation for specified purpose.
        /// </summary>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        public abstract IHttpClient Build(string clientPurpose);
    }
}
