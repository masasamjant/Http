using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Masasamjant.Http.Demo.Models
{
    public class CarSeachViewModel
    {
        [JsonInclude]
        public string Manufacturer { get; set; } = string.Empty;

        [JsonInclude]
        public string Model { get; set; } = string.Empty;

        [JsonInclude]
        [Display(Name = "Registration Number")]
        public string RegisterNumber { get; set; } = string.Empty;

        [JsonInclude]
        [Range(1900, 2025)]
        [Display(Name = "Model Year")]
        public int ModelYear { get; set; }

        [JsonInclude]
        [Range(1900, 2025)]
        [Display(Name = "Production Year")]
        public int ProductionYear { get; set; }

        [JsonInclude]
        [Range(2, 32)]
        public int Seats { get; set; }

        [JsonInclude]
        public EngineType Engine { get; set; } = EngineType.Petrol;

        [JsonInclude]
        [Display(Name = "Type")]
        public CarType CarType { get; set; } = CarType.Unspecified;
    }
}
