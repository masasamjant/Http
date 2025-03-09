namespace Masasamjant.Http.Listeners
{
    /// <summary>
    /// Represents arguments of <see cref="HttpRequestExecutionTimeListener.RequestExecuted"/> event.
    /// </summary>
    public sealed class HttpRequestExecutionTimeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequestExecutionTimeEventArgs"/> class.
        /// </summary>
        /// <param name="httpRequest">The key information of executed HTTP request.</param>
        /// <param name="executionTime">The execution time of HTTP request.</param>
        public HttpRequestExecutionTimeEventArgs(HttpRequestKey httpRequest, TimeSpan executionTime)
        {
            HttpRequest = httpRequest;
            ExecutionTime = executionTime;
        }

        /// <summary>
        /// Gets the key information of executed HTTP request.
        /// </summary>
        HttpRequestKey HttpRequest { get; }

        /// <summary>
        /// Gets the execution time of HTTP request.
        /// </summary>
        public TimeSpan ExecutionTime { get; }
    }
}
