using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Masasamjant.Http.Abstractions
{
    [TestClass]
    public class HttpHeaderRequestInterceptorUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_GetHttpRequestInterceptionCancelBehavior()
        {
            var interceptor = new TestHttpHeaderRequestInterceptor();
            HttpRequestInterceptionCancelBehavior expected = HttpRequestInterceptionCancelBehavior.Throw;
            HttpRequestInterceptionCancelBehavior actual = interceptor.TestGetHttpRequestInterceptionCancelBehavior(new Exception());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_AddHttpHeader()
        {
            var interceptor = new TestHttpHeaderRequestInterceptor();
            var request = new HttpGetRequest("/content", null);
            Func<string?> getName = () => null;
            Func<string?> getValue = () => "value";
            
            var interception = interceptor.TestAddHttpHeader(request, getName, getValue);
            Assert.IsTrue(request.Headers.Count == 0);
            Assert.AreEqual(HttpRequestInterceptionResult.Continue, interception.Result);

            getName = () => "name";

            interception = interceptor.TestAddHttpHeader(request, getName, getValue);
            Assert.IsTrue(request.Headers.Contains("name"));
            Assert.AreEqual(HttpRequestInterceptionResult.Continue, interception.Result);

            getName = () => "#-001Ä";
            interception = interceptor.TestAddHttpHeader(request, getName, getValue);
            Assert.AreEqual(HttpRequestInterceptionResult.Cancel, interception.Result);
            Assert.AreEqual(HttpRequestInterceptionCancelBehavior.Throw, interception.CancelBehavior);
        }

        private class TestHttpHeaderRequestInterceptor : HttpHeaderRequestInterceptor
        {
            public override Task<HttpRequestInterception> InterceptAsync(HttpGetRequest request)
            {
                throw new NotImplementedException();
            }

            public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
            {
                throw new NotImplementedException();
            }

            public HttpRequestInterceptionCancelBehavior TestGetHttpRequestInterceptionCancelBehavior(Exception exception)
            {
                return base.GetHttpRequestInterceptionCancelBehavior(exception);
            }

            public HttpRequestInterception TestAddHttpHeader(HttpRequest request, Func<string?> getName, Func<string?> getValue)
            {
                return base.AddHttpHeader(request, getName, getValue);
            }
        }
    }
}
