using IHttpClientBuilder = Masasamjant.Http.Abstractions.IHttpClientBuilder;
using Masasamjant.Http.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Masasamjant.Http.Abstractions;
using Masasamjant.Http.Listeners;
using Masasamjant.Http.Demo.Interceptors;
using HttpRequest = Masasamjant.Http.Abstractions.HttpRequest;

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
                var interceptor = new DebugRequestUriInterceptor();
                client.HttpGetRequestInterceptors.Add(interceptor);
                client.HttpPostRequestInterceptors.Add(interceptor);
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
            var parameters = GetSearchParameters(form);

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
            var request = new HttpGetRequest("api/GetCar", [HttpParameter.From("identifier", identifier.ToString())]);
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

        private static List<HttpParameter> GetSearchParameters(CarSeachViewModel form)
        {
            var parameters = new List<HttpParameter>();

            if (!string.IsNullOrWhiteSpace(form.Manufacturer))
                parameters.Add(HttpParameter.From("manufacturer", form.Manufacturer));

            if (!string.IsNullOrWhiteSpace(form.Model))
                parameters.Add(HttpParameter.From("model", form.Model));

            if (!string.IsNullOrWhiteSpace(form.RegisterNumber))
                parameters.Add(HttpParameter.From("registerNumber", form.RegisterNumber));

            if (form.ModelYear > 0)
                parameters.Add(HttpParameter.From("modelYear", form.ModelYear.ToString()));

            if (form.ProductionYear > 0)
                parameters.Add(HttpParameter.From("productionYear", form.ProductionYear.ToString()));

            if (form.Seats > 0)
                parameters.Add(HttpParameter.From("seats", form.Seats.ToString()));

            parameters.Add(HttpParameter.From("engine", form.Engine.ToString()));

            if (form.CarType != CarType.Unspecified)
                parameters.Add(HttpParameter.From("carType", form.CarType.ToString()));

            return parameters;
        }

        private static void AddRequestTimeHeader(HttpRequest request)
        {
            request.Headers.Add("X-Request-Identifier", request.Identifier.ToString());
            request.Headers.Add("X-Request-Time", DateTime.Now.ToString());
        }
    }
}
