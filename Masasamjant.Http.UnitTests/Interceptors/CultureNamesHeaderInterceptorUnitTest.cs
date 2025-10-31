using Masasamjant.Globalization;
using System.Globalization;

namespace Masasamjant.Http.Interceptors
{
    [TestClass]
    public class CultureNamesHeaderInterceptorUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var interceptor = new CultureNamesHeaderInterceptor(null, null);
            Assert.IsNull(interceptor.CurrentCultureHeaderName);
            Assert.IsNull(interceptor.CurrentCultureHeaderName);
            interceptor = new CultureNamesHeaderInterceptor("X-Culture", "X-UI-Culture");
            Assert.AreEqual("X-Culture", interceptor.CurrentCultureHeaderName);
            Assert.AreEqual("X-UI-Culture", interceptor.CurrentUICultureHeaderName);
        }

        [TestMethod]
        public async Task Test_Intercept_GetRequest()
        {
            var request = new HttpGetRequest("api/Test");
            var interceptor = new CultureNamesHeaderInterceptor(null, null);
            var interception = await interceptor.InterceptAsync(request);
            Assert.AreEqual(HttpRequestInterception.Continue, interception);
            var currentCultureHeader = request.Headers.Get("X-Culture");
            var currentUICultureHeader = request.Headers.Get("X-UI-Culture");
            Assert.IsNull(currentCultureHeader);
            Assert.IsNull(currentUICultureHeader);

            request = new HttpGetRequest("api/Test");
            interceptor = new CultureNamesHeaderInterceptor("", "");
            interception = await interceptor.InterceptAsync(request);
            Assert.AreEqual(HttpRequestInterception.Continue, interception);
            currentCultureHeader = request.Headers.Get("");
            currentUICultureHeader = request.Headers.Get("");
            Assert.IsNull(currentCultureHeader);
            Assert.IsNull(currentUICultureHeader);

            request = new HttpGetRequest("api/Test");
            interceptor = new CultureNamesHeaderInterceptor("  ", "  ");
            interception = await interceptor.InterceptAsync(request);
            Assert.AreEqual(HttpRequestInterception.Continue, interception);
            currentCultureHeader = request.Headers.Get("  ");
            currentUICultureHeader = request.Headers.Get("  ");
            Assert.IsNull(currentCultureHeader);
            Assert.IsNull(currentUICultureHeader);

            using (var context = new CultureContext(CultureInfo.GetCultureInfo("fi-FI"), CultureInfo.GetCultureInfo("en-US")))
            {
                request = new HttpGetRequest("api/Test");
                interceptor = new CultureNamesHeaderInterceptor("X-Culture", "X-UI-Culture");
                interception = await interceptor.InterceptAsync(request);
                Assert.AreEqual(HttpRequestInterception.Continue, interception);
                currentCultureHeader = request.Headers.Get("X-Culture");
                currentUICultureHeader = request.Headers.Get("X-UI-Culture");
                Assert.IsNotNull(currentCultureHeader);
                Assert.IsNotNull(currentUICultureHeader);
                Assert.AreEqual("X-Culture", currentCultureHeader.Name);
                Assert.AreEqual("fi-FI", currentCultureHeader.Value);
                Assert.AreEqual("X-UI-Culture", currentUICultureHeader.Name);
                Assert.AreEqual("en-US", currentUICultureHeader.Value);
            }

            using (var context = new CultureContext(CultureInfo.GetCultureInfo("fi-FI"), CultureInfo.GetCultureInfo("en-US")))
            {
                request = new HttpGetRequest("api/Test");
                interceptor = new CultureNamesHeaderInterceptor("X-Culture", "X-Culture");
                interception = await interceptor.InterceptAsync(request);
                Assert.AreEqual(HttpRequestInterception.Continue, interception);
                currentCultureHeader = request.Headers.Get("X-Culture");
                currentUICultureHeader = request.Headers.Get("X-Culture");
                Assert.IsNotNull(currentCultureHeader);
                Assert.IsTrue(ReferenceEquals(currentCultureHeader, currentUICultureHeader));
                Assert.AreEqual("X-Culture", currentCultureHeader.Name);
                Assert.AreEqual("fi-FI", currentCultureHeader.Value);
            }
        }

        [TestMethod]
        public async Task Test_Intercept_PostRequest()
        {
            var request = new HttpPostRequest("api/Test", 1);
            var interceptor = new CultureNamesHeaderInterceptor(null, null);
            var interception = await interceptor.InterceptAsync(request);
            Assert.AreEqual(HttpRequestInterception.Continue, interception);
            var currentCultureHeader = request.Headers.Get("X-Culture");
            var currentUICultureHeader = request.Headers.Get("X-UI-Culture");
            Assert.IsNull(currentCultureHeader);
            Assert.IsNull(currentUICultureHeader);

            request = new HttpPostRequest("api/Test", 1);
            interceptor = new CultureNamesHeaderInterceptor("", "");
            interception = await interceptor.InterceptAsync(request);
            Assert.AreEqual(HttpRequestInterception.Continue, interception);
            currentCultureHeader = request.Headers.Get("");
            currentUICultureHeader = request.Headers.Get("");
            Assert.IsNull(currentCultureHeader);
            Assert.IsNull(currentUICultureHeader);

            request = new HttpPostRequest("api/Test", 1);
            interceptor = new CultureNamesHeaderInterceptor("  ", "  ");
            interception = await interceptor.InterceptAsync(request);
            Assert.AreEqual(HttpRequestInterception.Continue, interception);
            currentCultureHeader = request.Headers.Get("  ");
            currentUICultureHeader = request.Headers.Get("  ");
            Assert.IsNull(currentCultureHeader);
            Assert.IsNull(currentUICultureHeader);

            using (var context = new CultureContext(CultureInfo.GetCultureInfo("fi-FI"), CultureInfo.GetCultureInfo("en-US")))
            {
                request = new HttpPostRequest("api/Test", 1);
                interceptor = new CultureNamesHeaderInterceptor("X-Culture", "X-UI-Culture");
                interception = await interceptor.InterceptAsync(request);
                Assert.AreEqual(HttpRequestInterception.Continue, interception);
                currentCultureHeader = request.Headers.Get("X-Culture");
                currentUICultureHeader = request.Headers.Get("X-UI-Culture");
                Assert.IsNotNull(currentCultureHeader);
                Assert.IsNotNull(currentUICultureHeader);
                Assert.AreEqual("X-Culture", currentCultureHeader.Name);
                Assert.AreEqual("fi-FI", currentCultureHeader.Value);
                Assert.AreEqual("X-UI-Culture", currentUICultureHeader.Name);
                Assert.AreEqual("en-US", currentUICultureHeader.Value);
            }

            using (var context = new CultureContext(CultureInfo.GetCultureInfo("fi-FI"), CultureInfo.GetCultureInfo("en-US")))
            {
                request = new HttpPostRequest("api/Test", 1);
                interceptor = new CultureNamesHeaderInterceptor("X-Culture", "X-Culture");
                interception = await interceptor.InterceptAsync(request);
                Assert.AreEqual(HttpRequestInterception.Continue, interception);
                currentCultureHeader = request.Headers.Get("X-Culture");
                currentUICultureHeader = request.Headers.Get("X-Culture");
                Assert.IsNotNull(currentCultureHeader);
                Assert.IsTrue(ReferenceEquals(currentCultureHeader, currentUICultureHeader));
                Assert.AreEqual("X-Culture", currentCultureHeader.Name);
                Assert.AreEqual("fi-FI", currentCultureHeader.Value);
            }
        }
    }
}
