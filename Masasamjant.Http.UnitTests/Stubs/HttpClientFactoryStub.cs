using System;
using System.Collections.Generic;
using System.Text;

namespace Masasamjant.Http.Stubs
{
    internal class HttpClientFactoryStub : IHttpClientFactory
    {
        private readonly HttpMessageHandlerStub handler;

        public HttpClientFactoryStub(HttpMessageHandlerStub handler)
        {
            this.handler = handler;
        }

        public System.Net.Http.HttpClient CreateClient(string name)
        {
            return new HttpClient(handler);
        }
    }
}
