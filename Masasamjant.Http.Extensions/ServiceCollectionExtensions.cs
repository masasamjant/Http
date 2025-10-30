using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Json;
using Masasamjant.Http.Xml;
using Microsoft.Extensions.DependencyInjection;
using IHttpClientBuilder = Masasamjant.Http.Abstractions.IHttpClientBuilder;

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
        public static IServiceCollection AddHttpClientFromConfiguration(this IServiceCollection services, HttpClientConfiguration? configuration = null)
        {
            return configuration != null ? configuration.AddHttpClient(services) : services;
        }

        /// <summary>
        /// Add singleton <see cref="IHttpClientBuilder"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="httpClientBuilder">The singleton <see cref="IHttpClientBuilder"/>.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpClientBuilder(this IServiceCollection services, IHttpClientBuilder httpClientBuilder)
            => services.AddSingleton(httpClientBuilder);

        /// <summary>
        /// Add transient <see cref="IHttpClientBuilder"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="implementationFactory">The delegate to factory method to create <see cref="IHttpClientBuilder"/> implementation instance.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpClientBuilder(this IServiceCollection services, Func<IServiceProvider, IHttpClientBuilder> implementationFactory)
            => services.AddTransient(implementationFactory);

        /// <summary>
        /// Add singleton <see cref="JsonHttpClientBuilder"/> as <see cref="IHttpClientBuilder"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="httpClientConfiguration">The <see cref="HttpClientConfiguration"/> if <see cref="HttpClient"/> is registered also; <c>null</c> otherwise.</param>
        /// <param name="httpBaseAddressProviderFactory">The singleton <see cref="IHttpBaseAddressProviderFactory"/> to register also; <c>null</c> otherwise.</param>
        /// <param name="httpCacheManager">The singleton <see cref="IHttpCacheManager"/> to register also; <c>null</c> otherwise.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddJsonHttpClientBuilder(this IServiceCollection services, HttpClientConfiguration? httpClientConfiguration = null, IHttpBaseAddressProviderFactory? httpBaseAddressProviderFactory = null, IHttpCacheManager? httpCacheManager = null)
        {
            if (httpClientConfiguration != null)
                httpClientConfiguration.AddHttpClient(services);

            if (httpBaseAddressProviderFactory != null)
                services.AddHttpBaseAddressProviderFactory(httpBaseAddressProviderFactory);

            if (httpCacheManager != null)
                services.AddHttpCacheManager(httpCacheManager);

            return services.AddSingleton<IHttpClientBuilder, JsonHttpClientBuilder>();
        }

        /// <summary>
        /// Add singleton <see cref="XmlHttpClientBuilder"/> as <see cref="IHttpClientBuilder"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="httpClientConfiguration">The <see cref="HttpClientConfiguration"/> if <see cref="HttpClient"/> is registered also; <c>null</c> otherwise.</param>
        /// <param name="httpBaseAddressProviderFactory">The singleton <see cref="IHttpBaseAddressProviderFactory"/> to register also; <c>null</c> otherwise.</param>
        /// <param name="httpCacheManager">The singleton <see cref="IHttpCacheManager"/> to register also; <c>null</c> otherwise.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddXmlHttpClientBuilder(this IServiceCollection services, HttpClientConfiguration? httpClientConfiguration = null, IHttpBaseAddressProviderFactory? httpBaseAddressProviderFactory = null, IHttpCacheManager? httpCacheManager = null)
        {
            if (httpClientConfiguration != null)
                httpClientConfiguration.AddHttpClient(services);

            if (httpBaseAddressProviderFactory != null)
                services.AddHttpBaseAddressProviderFactory(httpBaseAddressProviderFactory);

            if (httpCacheManager != null)
                services.AddHttpCacheManager(httpCacheManager);

            return services.AddSingleton<IHttpClientBuilder, XmlHttpClientBuilder>();
        }

        /// <summary>
        /// Add singleton <see cref="IHttpBaseAddressProviderFactory"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="httpBaseAddressProviderFactory">The singleton <see cref="IHttpBaseAddressProviderFactory"/>.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpBaseAddressProviderFactory(this IServiceCollection services, IHttpBaseAddressProviderFactory httpBaseAddressProviderFactory)
            => services.AddSingleton(httpBaseAddressProviderFactory);

        /// <summary>
        /// Add transient <see cref="IHttpBaseAddressProviderFactory"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="implementationFactory">The delegate to factory method to create <see cref="IHttpBaseAddressProviderFactory"/> implementation instance.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpBaseAddressProviderFactory(this IServiceCollection services, Func<IServiceProvider, IHttpBaseAddressProviderFactory> implementationFactory)
            => services.AddTransient(implementationFactory);

        /// <summary>
        /// Add singleton <see cref="IHttpCacheManager"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="httpCacheManager">The singleton <see cref="IHttpCacheManager"/>.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpCacheManager(this IServiceCollection services, IHttpCacheManager httpCacheManager) => services.AddSingleton(httpCacheManager);

        /// <summary>
        /// Add transient <see cref="IHttpCacheManager"/> to specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="implementationFactory">The delegate to factory method to create <see cref="IHttpCacheManager"/> implementation instance.</param>
        /// <returns>A <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpCacheManager(this IServiceCollection services, Func<IServiceProvider, IHttpCacheManager> implementationFactory) => services.AddTransient(implementationFactory);
    }
}
