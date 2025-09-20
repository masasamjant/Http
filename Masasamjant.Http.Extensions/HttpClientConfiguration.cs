using Microsoft.Extensions.DependencyInjection;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents configuring <see cref="IServiceCollection"/> to contain <see cref="HttpClient"/>.
    /// </summary>
    public sealed class HttpClientConfiguration
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpClientConfiguration"/> class.
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="clientConfiguration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HttpClientConfiguration(string? clientName = null, Action<HttpClient>? clientConfiguration = null)
        {
            if (clientConfiguration != null && string.IsNullOrWhiteSpace(clientName))
                throw new ArgumentNullException(nameof(clientName), "The client name must be specified when configuration delegate it not null.");

            ClientName = string.IsNullOrWhiteSpace(clientName) ? null : clientName;
            ClientConfiguration = clientConfiguration;
        }

        /// <summary>
        /// Gets the name of HTTP client.
        /// </summary>
        public string? ClientName { get; }

        /// <summary>
        /// Gets the delegate to configure HTTP client.
        /// </summary>
        public Action<HttpClient>? ClientConfiguration { get; }

        /// <summary>
        /// Adds <see cref="HttpClient"/> to specified <see cref="IServiceCollection"/> using value of this class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add <see cref="HttpClient"/>.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public IServiceCollection AddHttpClient(IServiceCollection services)
        {
            if (!string.IsNullOrWhiteSpace(ClientName))
            {
                if (ClientConfiguration != null)
                    services.AddHttpClient(ClientName, ClientConfiguration);
                else
                    services.AddHttpClient(ClientName);
            }
            else
                services.AddHttpClient();
            
            return services;
        }
    }
}
