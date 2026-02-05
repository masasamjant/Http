using Masasamjant.Http.Interceptors;

namespace Masasamjant.Http
{
    [TestClass]
    public class HttpClientExtensionsUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_AddRequestIdentifierHeaderInterceptor()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddRequestIdentifierHeaderInterceptor(client, "Test");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as HttpRequestIdentifierHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
            Assert.IsTrue(headerInterceptor.RequestIdentifierHeaderName == "Test");
        }

        [TestMethod]
        public void Test_AddCultureNamesHeaderInterceptor()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddCultureNamesHeaderInterceptor(client, "Current-Culture", "Current-UI-Culture");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as CultureNamesHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
            Assert.IsTrue(headerInterceptor.CurrentCultureHeaderName == "Current-Culture");
            Assert.IsTrue(headerInterceptor.CurrentUICultureHeaderName == "Current-UI-Culture");
        }

        [TestMethod]
        public void Test_AddApiKeyHeaderInterceptor_With_Static_Name_And_Value()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddApiKeyHeaderInterceptor(client, "Api-Key", "12345");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as ApiKeyHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
        }

        [TestMethod]
        public void Test_AddApiKeyHeaderInterceptor_With_Static_Name()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddApiKeyHeaderInterceptor(client, "Api-Key", () => "12345");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as ApiKeyHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
        }

        [TestMethod]
        public void Test_AddApiKeyHeaderInterceptor_With_Dynamic_Name_And_Value()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddApiKeyHeaderInterceptor(client, () => "Api-Key", () => "12345");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as ApiKeyHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
        }

        [TestMethod]
        public void Test_AddAuthenticationTokenHeaderInterceptor_With_Static_Name_And_Value()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddAuthenticationTokenHeaderInterceptor(client, "Auth-Token", "abcdef");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as AuthenticationTokenHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
        }

        [TestMethod]
        public void Test_AddAuthenticationTokenHeaderInterceptor_With_Statis_Name()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddAuthenticationTokenHeaderInterceptor(client, "Auth-Token", () => "abcdef");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as AuthenticationTokenHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
        }

        [TestMethod]
        public void Test_AddAuthenticationTokenHeaderInterceptor_With_Dynamic_Name_And_Value()
        {
            var client = new TestHttpClient();
            HttpClientExtensions.AddAuthenticationTokenHeaderInterceptor(client, () => "Auth-Token", () => "abcdef");
            Assert.IsTrue(client.HttpGetRequestInterceptors.Count == 1);
            Assert.IsTrue(client.HttpPostRequestInterceptors.Count == 1);
            var getInterceptor = client.HttpGetRequestInterceptors.First();
            var postInterceptor = client.HttpPostRequestInterceptors.First();
            Assert.IsTrue(ReferenceEquals(getInterceptor, postInterceptor));
            var headerInterceptor = getInterceptor as AuthenticationTokenHeaderInterceptor;
            Assert.IsNotNull(headerInterceptor);
        }
    }
}
