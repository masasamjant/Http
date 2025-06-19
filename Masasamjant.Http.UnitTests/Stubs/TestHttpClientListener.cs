using Masasamjant.Http.Abstractions;
using System.Text;

namespace Masasamjant.Http
{
    internal class TestHttpClientListener : HttpClientListener
    {
        private readonly StringBuilder builder;

        public TestHttpClientListener(StringBuilder builder)
        {
            this.builder = builder;
        }

        public override Task OnErrorAsync(HttpRequest request, Exception exception)
        {
            builder.Append($"Error: {exception.Message}");
            return Task.CompletedTask;
        }

        public override Task OnExecutedAsync(HttpRequest request)
        {
            builder.Append($"Executed: {request.Identifier}");
            return Task.CompletedTask;
        }

        public override Task OnExecutingAsync(HttpRequest request)
        {
            builder.Append($"Executing: {request.Identifier}");
            return Task.CompletedTask;
        }
    }
}
