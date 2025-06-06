using Masasamjant.Http.Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Masasamjant.Http.Demo.Controllers
{
    [ApiController]
    public class CarsController : ControllerBase
    {
        private static readonly List<CarViewModel> carList = new List<CarViewModel>();

        [HttpGet]
        [Route("api/GetCars")]
        public IActionResult GetCars()
        {
            WriteRequestTimeHeader();

            return Ok(carList);
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

            return Ok(cars.ToList());
        }

        [HttpGet]
        [Route("api/GetCar")]
        public IActionResult GetCar([FromQuery] Guid identifier)
        {
            WriteRequestTimeHeader();
            var car = carList.FirstOrDefault(c => c.Identifier == identifier);
            if (car == null)
                return NotFound();
            return Ok(car);
        }

        [HttpPost]
        [Route("api/AddCar")]
        public IActionResult AddCar([FromBody] CarViewModel model)
        {
            WriteRequestTimeHeader();
            if (model == null)
                return BadRequest("Car model cannot be null.");
            carList.Add(model);
            return Ok(model);
        }

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
