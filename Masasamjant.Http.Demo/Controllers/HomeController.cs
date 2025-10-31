using IHttpClientBuilder = Masasamjant.Http.Abstractions.IHttpClientBuilder;
using Masasamjant.Http.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Listeners;
using Masasamjant.Http.Demo.Interceptors;
using HttpRequest = Masasamjant.Http.Abstractions.HttpRequest;
using Masasamjant.Http.Caching;

namespace Masasamjant.Http.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientBuilder httpClientBuilder;

        public HomeController(IHttpClientBuilder httpClientBuilder)
        {
            this.httpClientBuilder = httpClientBuilder;
        }

        private IHttpClient HttpClient
        {
            get
            {
                var client = httpClientBuilder.Build("Demo");
                
                // Add interceptor that writes request URI in debug.
                var interceptor = new DebugRequestUriInterceptor();
                client.HttpGetRequestInterceptors.Add(interceptor);
                client.HttpPostRequestInterceptors.Add(interceptor);

                // Add interceptor that adds request identifier to HTTP header.
                client.AddRequestIdentifierHeaderInterceptor("X-Request-Identifier");

                //Add interceptor that add culture names to HTTP header.
                client.AddCultureNamesHeaderInterceptor("X-Current-Culture", "X-Current-UI-Culture");
                
                return client;
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            SearchVisibile(false);
            var request = new HttpGetRequest("api/GetCars");
            AddRequestTimeHeader(request);
            var result = await HttpClient.GetAsync<IEnumerable<CarViewModel>>(request);
            return View(result ?? Enumerable.Empty<CarViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm] CarSeachViewModel form)
        {
            var client = HttpClient;

            // Add listener to measure execution time of the search request
            var listener = new HttpRequestExecutionTimeListener();
            TimeSpan? executionTime = null;
            listener.RequestExecuted += (s, e) =>
            {
                executionTime = e.ExecutionTime;
            };
            client.HttpClientListeners.Add(listener);

            // Indicate that search is visible
            SearchVisibile(true);

            // Convert model to HTTP parameters.
            var parameters = HttpParameterCollection.Create(form);

            // Create request to perform search.
            var request = new HttpGetRequest("api/SearchCars", parameters);
            AddRequestTimeHeader(request);

            // Execute the request and get results.
            var result = await client.GetAsync<IEnumerable<CarViewModel>>(request);

            // Record the execution time if available.
            if (executionTime.HasValue)
                ViewBag.SearchExecutionTime = executionTime.Value.TotalMilliseconds.ToString("F2") + " milliseconds";
            else
                ViewBag.SearchExecutionTime = "";

            // Return the view with the search results.
            return View("List", result ?? Enumerable.Empty<CarViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Car(Guid identifier)
        {
            // Since no possiblity to update or remove cars, cache single result.
            var caching = new HttpGetRequestCaching(true, TimeSpan.FromMinutes(5));

            var request = new HttpGetRequest("api/GetCar", [HttpParameter.From("identifier", identifier.ToString())], caching);
            
            AddRequestTimeHeader(request);
            var result = await HttpClient.GetAsync<CarViewModel>(request);
            if (result == null)
                return RedirectToAction("List");
            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new CarViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CarViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            // Create interceptor to ensure that car identifier is set before sending the request.
            var client = HttpClient;
            var interceptor = new CarIdentifierPostRequestInterceptor() 
            {
                CancelRequestWhenIdenfierIsEmpty = !form.GenerateNewIdentifier // When false, then cancel the request when identifier is empty and throw exception.
            };
            client.HttpPostRequestInterceptors.Add(interceptor);

            var request = new HttpPostRequest<CarViewModel>("api/AddCar", form);
            AddRequestTimeHeader(request);
            request.Canceled += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debug.WriteLine($"POST request '{request.Identifier}' was canceled.");
            };
            var result = await client.PostAsync(request);
            if (result == null)
                return RedirectToAction("List");
            return RedirectToAction("Car", new { identifier = result.Identifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void SearchVisibile(bool visible)
        {
            ViewData["SearchVisible"] = visible;
        }

        private static void AddRequestTimeHeader(HttpRequest request)
        {
            request.Headers.Add("X-Request-Time", DateTime.Now.ToString());
        }
    }
}
