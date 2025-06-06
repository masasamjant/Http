using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Demo.Models;

namespace Masasamjant.Http.Demo.Interceptors
{
    /// <summary>
    /// Represents an HTTP Post request interceptor that adds a car identifier to the request data.
    /// </summary>
    public class CarIdentifierPostRequestInterceptor : HttpPostRequestInterceptor
    {
        /// <summary>
        /// Gets or sets a value indicating whether to cancel the request when the car identifier is empty.
        /// If <c>true</c>, then request will be canceled and an exception will be thrown if the identifier is empty.
        /// If <c>false</c>, then a new identifier will be generated for the car model.
        /// </summary>
        public bool CancelRequestWhenIdenfierIsEmpty { get; set; }

        /// <summary>
        /// Intercepts the specified <see cref="HttpPostRequest"/> to add a car identifier.
        /// </summary>
        /// <param name="request">The <see cref="HttpPostRequest"/> to intercept.</param>
        /// <returns>A <see cref="HttpRequestInterception"/> after this interceptor.</returns>
        public override Task<HttpRequestInterception> InterceptAsync(HttpPostRequest request)
        {
            // If the request data is car model and identifier is not set, generate a new one.
            if (request.Data is CarViewModel car && car.Identifier == Guid.Empty)
            {
                if (CancelRequestWhenIdenfierIsEmpty)
                {
                    return Task.FromResult(HttpRequestInterception.Cancel(HttpRequestInterceptionCancelBehavior.Throw, "Car idenfier is empty GUID."));
                }

                car.Identifier = Guid.NewGuid();
            }

            return Task.FromResult(HttpRequestInterception.Continue);
        }
    }
}
