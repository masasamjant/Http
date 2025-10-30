using Masasamjant.Http.Demo.Models;
using Masasamjant.Xml;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Masasamjant.Http.Demo.Controllers
{
    [ApiController]
    public class CarsController : ControllerBase
    {
        private static readonly List<CarViewModel> carList = new List<CarViewModel>();

        static CarsController()
        {
            carList.Add(new CarViewModel
            {
                Identifier = Guid.NewGuid(),
                Manufacturer = "Toyota",
                Model = "Corolla",
                RegisterNumber = "ABC-123",
                ModelYear = 2020,
                ProductionYear = 2019,
                Seats = 5,
                Engine = EngineType.Petrol,
                CarType = CarType.Sedan
            });
            carList.Add(new CarViewModel
            {
                Identifier = Guid.NewGuid(),
                Manufacturer = "Tesla",
                Model = "Model S",
                RegisterNumber = "XYZ-789",
                ModelYear = 2021,
                ProductionYear = 2020,
                Seats = 5,
                Engine = EngineType.Electric,
                CarType = CarType.Sedan
            });
            carList.Add(new CarViewModel
            {
                Identifier = Guid.NewGuid(),
                Manufacturer = "Ford",
                Model = "Mustang",
                RegisterNumber = "MST-456",
                ModelYear = 2022,
                ProductionYear = 2021,
                Seats = 4,
                Engine = EngineType.Petrol,
                CarType = CarType.Coupe
            });
            carList.Add(new CarViewModel
            {
                Identifier = Guid.NewGuid(),
                Manufacturer = "Volkswagen",
                Model = "Golf",
                RegisterNumber = "GOL-321",
                ModelYear = 2019,
                ProductionYear = 2018,
                Seats = 5,
                Engine = EngineType.Diesel,
                CarType = CarType.Hatchback
            });
        }

        private readonly string contentType;

        public CarsController(IConfiguration configuration)
        {
            var section = configuration.GetRequiredSection("HttpClient");
            var clientType = section["Type"];
            contentType = string.Equals(clientType, "json", StringComparison.OrdinalIgnoreCase) ? "application/json" : "application/xml";
        }

        [HttpGet]
        [Route("api/GetCars")]
        public IActionResult GetCars()
        {
            WriteRequestTimeHeader();

            var content = Serialize(carList);

            return OkResult(content);
        }

        [HttpGet]
        [Route("api/SearchCars")]
        public IActionResult SearchCars(string? manufacturer = null, string? model = null, string? registerNumber = null, int modelYear = 0, int productionYear = 0, int seats = 0, EngineType engine = EngineType.Petrol, CarType carType = CarType.Unspecified)
        {
            WriteRequestTimeHeader();

            var cars = carList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(manufacturer))
                cars = cars.Where(c => c.Manufacturer == manufacturer);

            if (!string.IsNullOrWhiteSpace(model))
                cars = cars.Where(c => c.Model == model);

            if (!string.IsNullOrWhiteSpace(registerNumber))
                cars = cars.Where(c => c.RegisterNumber == registerNumber);

            if (modelYear > 0)
                cars = cars.Where(c => c.ModelYear == modelYear);

            if (productionYear > 0)
                cars = cars.Where(c => c.ProductionYear == productionYear);

            if (seats > 0)
                cars = cars.Where(c => c.Seats == seats);

            cars = cars.Where(c => c.Engine == engine);

            if (carType != CarType.Unspecified)
                cars = cars.Where(c => c.CarType == carType);

            var content = Serialize(cars.ToList());

            return OkResult(content);
        }

        [HttpGet]
        [Route("api/GetCar")]
        public IActionResult GetCar([FromQuery] Guid identifier)
        {
            WriteRequestTimeHeader();
            var car = carList.FirstOrDefault(c => c.Identifier == identifier);
            if (car == null)
                return NotFound();

            var content = Serialize(car);

            return OkResult(content);
        }

        [HttpPost]
        [Route("api/AddCar")]
        public IActionResult AddCar([FromBody] CarViewModel model)
        {
            WriteRequestTimeHeader();
            
            if (model == null)
                return BadRequest("Car model cannot be null.");
            
            carList.Add(model);
            
            var content = Serialize(model);

            return OkResult(content);
        }

        private ContentResult OkResult(string content)
            => new ContentResult() 
            {
                ContentType = contentType,
                Content = content,
                StatusCode = 200
            };

        private string Serialize(CarViewModel model)
        {
            if (UseJsonSerialization())
            {
                return JsonSerializer.Serialize(model);
            }
            else
            {
                var factory = new XmlSerializerFactory(XmlSerialization.Contract);
                var serializer = factory.CreateSerializer(model.GetType());
                return serializer.Serialize(model);
            }
        }

        private string Serialize(List<CarViewModel> cars)
        {
            if (UseJsonSerialization())
            {
                return JsonSerializer.Serialize(cars);
            }
            else
            {
                var factory = new XmlSerializerFactory(XmlSerialization.Contract);
                var serializer = factory.CreateSerializer(cars.GetType());
                return serializer.Serialize(cars);
            }
        }

        private bool UseJsonSerialization() => string.Equals(contentType, "application/json", StringComparison.OrdinalIgnoreCase);

        private void WriteRequestTimeHeader()
        {
            if (HttpContext.Request.Headers.TryGetValue("X-Request-Time", out var requestTime) && requestTime.Count > 0 &&
                HttpContext.Request.Headers.TryGetValue("X-Request-Identifier", out var requestIdentifier) && requestIdentifier.Count > 0)
            {
               if (Debugger.IsAttached)
                    Debug.WriteLine($"Request '{requestIdentifier[0]}' at '{requestTime[0]}'.");
            }
        }
    }
}
