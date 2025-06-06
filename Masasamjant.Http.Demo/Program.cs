using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Json;
using IHttpClientBuilder = Masasamjant.Http.Abstractions.IHttpClientBuilder;

namespace Masasamjant.Http.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IHttpBaseAddressProviderFactory>(new HttpBaseAddressProviderFactory(builder.Configuration, "HttpClient"));
            builder.Services.AddSingleton<IHttpClientBuilder, JsonHttpClientBuilder>();

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
