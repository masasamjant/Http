using Masasamjant.Http.Abstractions;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Masasamjant.Http.Listeners
{
    /// <summary>
    /// Represents HTTP client listener that records execution times of HTTP requests.
    /// </summary>
    public sealed class HttpRequestExecutionTimeListener : HttpClientListener
    {
        private readonly ConcurrentDictionary<HttpRequestKey, Stopwatch> requestStopwatches;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpRequestExecutionTimeListener"/> class.
        /// </summary>
        public HttpRequestExecutionTimeListener()
        {
            requestStopwatches = new ConcurrentDictionary<HttpRequestKey, Stopwatch>();
        }

        /// <summary>
        /// Notifies when HTTP request has been executed.
        /// </summary>
        public event EventHandler<HttpRequestExecutionTimeEventArgs>? RequestExecuted;

        /// <summary>
        /// Invoked if exception occurs when executing specified HTTP request. Removes and stops 
        /// internal <see cref="Stopwatch"/> associated with HTTP request, but does not raise <see cref="RequestExecuted"/> event.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="exception">The occurred exception.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public override Task OnErrorAsync(HttpRequest request, Exception exception)
        {
            if (requestStopwatches.TryRemove(request.GetHttpRequestKey(), out Stopwatch? stopwatch))
                stopwatch.Stop();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Invoked after specified HTTP request is executed. Removes and stops 
        /// internal <see cref="Stopwatch"/> associated with HTTP request and raise <see cref="RequestExecuted"/> event.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public override Task OnExecutedAsync(HttpRequest request)
        {
            var requestKey = request.GetHttpRequestKey();

            if (requestStopwatches.TryRemove(requestKey, out Stopwatch? stopwatch)) 
            {
                stopwatch.Stop();
                RequestExecuted?.Invoke(this, new HttpRequestExecutionTimeEventArgs(requestKey, stopwatch.Elapsed));
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Invoked before specified HTTP request is executed. Registers internal <see cref="Stopwatch"/> with 
        /// HTTP request and stars recording execution time.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public override Task OnExecutingAsync(HttpRequest request)
        {
            var stopwatch = new Stopwatch();
            
            if (requestStopwatches.TryAdd(request.GetHttpRequestKey(), stopwatch))
                stopwatch.Start();

            return Task.CompletedTask;
        }
    }
}
