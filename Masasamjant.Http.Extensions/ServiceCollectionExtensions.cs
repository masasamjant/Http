using Microsoft.Extensions.DependencyInjection;

namespace Masasamjant.Http
{
    /// <summary>
    /// Provides extension methods to <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="HttpClient"/> to specified <see cref="IServiceCollection"/> using <see cref="HttpClientConfiguration"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="HttpClientConfiguration"/> or <c>null</c>, if <see cref="HttpClient"/> not configured.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpClient(this IServiceCollection services, HttpClientConfiguration? configuration = null)
        {
            return configuration != null ? configuration.AddHttpClient(services) : services;
        }
    }
}
