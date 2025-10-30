using Masasamjant.Http.Caching;

namespace Masasamjant.Http.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddXmlDataContractSerializerFormatters();

            var section = builder.Configuration.GetRequiredSection("HttpClient");
            var clientType = section["Type"];

            if (string.Equals(clientType, "json", StringComparison.OrdinalIgnoreCase))
            {
                // Register JSON Http builder and dependencies.
                builder.Services.AddJsonHttpClientBuilder(new HttpClientConfiguration(),
                    new ConfigurationHttpBaseAddressProviderFactory(builder.Configuration, "HttpClient"),
                    new MemoryHttpCacheManager());
            }
            else
            {
                // Register XML Http builder and dependencies.
                builder.Services.AddXmlHttpClientBuilder(new HttpClientConfiguration(),
                    new ConfigurationHttpBaseAddressProviderFactory(builder.Configuration, "HttpClient"),
                    new MemoryHttpCacheManager());
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
