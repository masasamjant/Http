using Masasamjant.Http.Abstractions;
using System.Diagnostics;

namespace Masasamjant.Http.Demo.Interceptors
{
    public class DebugRequestUriInterceptor : HttpRequestInterceptor
    {
        public override Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request)
        {
            if (Debugger.IsAttached)
                Debug.WriteLine($"GET: {request.FullRequestUri}");
            
            return Task.FromResult(HttpRequestInterception.Continue);
        }

        public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
        {
            if (Debugger.IsAttached)
                Debug.WriteLine($"POST: {request.RequestUri}");
            
            return Task.FromResult(HttpRequestInterception.Continue);
        }
    }
}
