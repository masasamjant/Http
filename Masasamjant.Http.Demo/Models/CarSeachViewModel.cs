using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Masasamjant.Http.Demo.Models
{
    public class CarSeachViewModel
    {
        [JsonInclude]
        [HttpParameter("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [JsonInclude]
        [HttpParameter("model")]
        public string Model { get; set; } = string.Empty;

        [JsonInclude]
        [Display(Name = "Registration Number")]
        [HttpParameter("registerNumber")]
        public string RegisterNumber { get; set; } = string.Empty;

        [JsonInclude]
        [Range(1900, 2025)]
        [Display(Name = "Model Year")]
        [HttpParameter("modelYear")]
        public int ModelYear { get; set; }

        [JsonInclude]
        [Range(1900, 2025)]
        [Display(Name = "Production Year")]
        [HttpParameter("productionYear")]
        public int ProductionYear { get; set; }

        [JsonInclude]
        [Range(2, 32)]
        [HttpParameter("seats")]
        public int Seats { get; set; }

        [JsonInclude]
        [HttpParameter("engine")]
        public EngineType Engine { get; set; } = EngineType.Petrol;

        [JsonInclude]
        [Display(Name = "Type")]
        [HttpParameter("carType")]
        public CarType CarType { get; set; } = CarType.Unspecified;
    }
}
