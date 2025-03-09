namespace Masasamjant.Http.Abstractions
{
    /// <summary>
    /// Represents interceptor of <see cref="HttpGetRequest"/>. Interceptor is active component that 
    /// can alter HTTP request or interfere with request processing.
    /// </summary>
    public interface IHttpGetRequestInterceptor : IHttpRequestInterceptor<HttpGetRequest>
    {
    }
}
