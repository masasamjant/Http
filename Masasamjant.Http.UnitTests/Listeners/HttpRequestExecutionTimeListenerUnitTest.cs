namespace Masasamjant.Http.Listeners
{
    [TestClass]
    public class HttpRequestExecutionTimeListenerUnitTest : UnitTest
    {
        [TestMethod]
        public async Task Test_Executing_Executed()
        {
            var listener = new HttpRequestExecutionTimeListener();
            var request = new HttpGetRequest("api/Test");
            TimeSpan? time = null;
            HttpRequestKey? key = null;
            listener.RequestExecuted += (s, e) => 
            {
                time = e.ExecutionTime;
                key = e.HttpRequest;
            };
            await listener.OnExecutingAsync(request);
            Thread.Sleep(1000);
            await listener.OnExecutedAsync(request);
            Assert.IsTrue(time.HasValue);
            Assert.AreEqual(key, request.GetHttpRequestKey());
        }

        [TestMethod]
        public async Task Test_Executing_Error_Executed()
        {
            var listener = new HttpRequestExecutionTimeListener();
            var request = new HttpGetRequest("api/Test");
            TimeSpan? time = null;
            listener.RequestExecuted += (s, e) =>
            {
                time = e.ExecutionTime;
            };
            await listener.OnExecutingAsync(request);
            Thread.Sleep(1000);
            await listener.OnErrorAsync(request, new InvalidOperationException("Test"));
            Thread.Sleep(1000);
            await listener.OnExecutedAsync(request);
            Assert.IsFalse(time.HasValue);
        }
    }
}
