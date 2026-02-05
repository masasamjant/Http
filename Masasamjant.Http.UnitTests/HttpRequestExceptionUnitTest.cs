namespace Masasamjant.Http
{
    [TestClass]
    public class HttpRequestExceptionUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var request = new HttpGetRequest("api/Test");
            var exception = new HttpRequestException(request);
            Assert.IsTrue(ReferenceEquals(request, exception.HttpRequest));
            Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
            Assert.IsNull(exception.InnerException);

            exception = new HttpRequestException(request, "GET");
            Assert.IsTrue(ReferenceEquals(request, exception.HttpRequest));
            Assert.AreEqual("GET", exception.Message);
            Assert.IsNull(exception.InnerException);

            var inner = new InvalidOperationException();
            exception = new HttpRequestException(request, "GET", inner);
            Assert.IsTrue(ReferenceEquals(request, exception.HttpRequest));
            Assert.AreEqual("GET", exception.Message);
            Assert.IsTrue(ReferenceEquals(inner, exception.InnerException));
        }
    }
}
