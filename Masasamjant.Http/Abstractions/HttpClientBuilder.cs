using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents abstract component to build instance of <see cref="IHttpClient"/> implementation.
    /// </summary>
    public abstract class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly IConfiguration? configuration;
        private readonly IHttpClientFactory? httpClientFactory;
        private readonly IHttpBaseAddressProviderFactory? httpBaseAddressProviderFactory;
        private readonly IHttpCacheManager? httpCacheManager;
        
        /// <summary>
        /// Initializes new instance of the <see cref="HttpClientBuilder"/> class with specified dependencies.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> or <c>null</c>, if not needed.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> or <c>null</c>, if not needed.</param>
        /// <param name="httpBaseAddressProviderFactory">The <see cref="IHttpBaseAddressProviderFactory"/> or <c>null</c>, if not needed.</param>
        /// <param name="httpCacheManager">The <see cref="IHttpCacheManager"/> or <c>null</c>, if not needed.</param>
        protected HttpClientBuilder(IConfiguration? configuration, IHttpClientFactory? httpClientFactory, IHttpBaseAddressProviderFactory? httpBaseAddressProviderFactory, IHttpCacheManager? httpCacheManager)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
            this.httpBaseAddressProviderFactory = httpBaseAddressProviderFactory;
            this.httpCacheManager = httpCacheManager;
        }

        /// <summary>
        /// Initializes new default instance of the <see cref="HttpClientBuilder"/> class.
        /// </summary>
        protected HttpClientBuilder()
        { }

        /// <summary>
        /// Gets the <see cref="IHttpClientFactory"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">If has not been set by constructor.</exception>
        protected virtual IHttpClientFactory HttpClientFactory 
        {
            get
            {
                if (httpClientFactory == null)
                    throw new InvalidOperationException("HTTP client factory not set. Use constructor to set or override property.");

                return httpClientFactory;
            }
        }

        /// <summary>
        /// Gets the <see cref="IHttpBaseAddressProviderFactory"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">If has not been set by constructor.</exception>
        protected IHttpBaseAddressProviderFactory HttpBaseAddressProviderFactory 
        {
            get
            {
                if (httpBaseAddressProviderFactory == null)
                    throw new InvalidOperationException("HTTP base address provider factory not set. Use constructor to set or override property.");

                return httpBaseAddressProviderFactory;
            }
        }

        /// <summary>
        /// Gets the <see cref="IConfiguration"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">If has not been set by constructor.</exception>
        protected virtual IConfiguration Configuration 
        {
            get
            {
                if (configuration == null)
                    throw new InvalidOperationException("Configuration is not set. Use constructor to set or override property.");

                return configuration;
            }
        }

        /// <summary>
        /// Gets the <see cref="IHttpCacheManager"/>.
        /// </summary>
        protected IHttpCacheManager CacheManager 
        {
            get 
            {
                if (httpCacheManager == null)
                    return HttpCacheManager.Default;

                return httpCacheManager;
            } 
        }

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
