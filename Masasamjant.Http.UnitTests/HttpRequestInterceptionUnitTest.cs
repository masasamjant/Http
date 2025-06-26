namespace Masasamjant.Http
{
    [TestClass]
    public class HttpRequestInterceptionUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Continue()
        {
            var interception = HttpRequestInterception.Continue;
            Assert.AreEqual(HttpRequestInterceptionResult.Continue, interception.Result);
            Assert.AreEqual(HttpRequestInterceptionCancelBehavior.Return, interception.CancelBehavior);
            Assert.IsNull(interception.CancelReason);
            Assert.IsFalse(interception.CancelRequest);
            Assert.IsFalse(interception.ThrowCancelException);
        }

        [TestMethod]
        public void Test_Cancel()
        {
            var interception = HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Return, "Test");
            Assert.AreEqual(HttpRequestInterceptionResult.Cancel, interception.Result);
            Assert.AreEqual(HttpRequestInterceptionCancelBehavior.Return, interception.CancelBehavior);
            Assert.AreEqual("Test", interception.CancelReason);
            Assert.IsTrue(interception.CancelRequest);
            Assert.IsFalse (interception.ThrowCancelException);

            interception = HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Throw, "Test");
            Assert.AreEqual(HttpRequestInterceptionResult.Cancel, interception.Result);
            Assert.AreEqual(HttpRequestInterceptionCancelBehavior.Throw, interception.CancelBehavior);
            Assert.AreEqual("Test", interception.CancelReason);
            Assert.IsTrue(interception.CancelRequest);
            Assert.IsTrue(interception.ThrowCancelException);
        }
    }
}
