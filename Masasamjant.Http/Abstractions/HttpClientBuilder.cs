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
        public IHttpClient Build(string clientPurpose)
        {
            var client = CreateClient(clientPurpose);
            ConfigureClient(client, clientPurpose);
            return client;
        }

        /// <summary>
        /// Derived classes must implement this method to create an instance of <see cref="IHttpClient"/> for the specified purpose.
        /// </summary>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        /// <returns>A <see cref="IHttpClient"/>.</returns>
        protected abstract IHttpClient CreateClient(string clientPurpose);

        /// <summary>
        /// Derived classes can override this method to configure the created <see cref="IHttpClient"/> instance before it is used.
        /// </summary>
        /// <param name="client">The <see cref="IHttpClient"/> obtained from <see cref="CreateClient(string)"/>.</param>
        /// <param name="clientPurpose">The purpose of the HTTP client.</param>
        protected virtual void ConfigureClient(IHttpClient client, string clientPurpose)
        {
            return;
        }
    }
}
