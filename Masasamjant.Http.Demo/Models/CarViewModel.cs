using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Masasamjant.Http.Demo.Models
{
    public class CarViewModel
    {
        [JsonInclude]
        public Guid Identifier { get; set; } = Guid.Empty;

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
        public int ModelYear { get; set; } = DateTime.Today.Year;

        [JsonInclude]
        [Range(1900, 2025)]
        [Display(Name = "Production Year")]
        public int ProductionYear { get; set; } = DateTime.Today.Year;

        [JsonInclude]
        [Range(2, 32)]
        public int Seats { get; set; } = 5;

        [JsonInclude]
        public EngineType Engine { get; set; } = EngineType.Petrol;

        [JsonInclude]
        [Display(Name = "Type")]
        public CarType CarType { get; set; } = CarType.Unspecified;

        public bool GenerateNewIdentifier { get; set; }
    }

    public enum EngineType
    {
        Petrol = 0,
        Diesel = 1,
        Electric = 2,
        Hybrid = 4
    }

    public enum CarType
    {
        Unspecified = 0,
        Sedan = 1,
        Hatchback = 2,
        SUV = 3,
        Coupe = 4,
        Convertible = 5,
        Minivan = 6,
        PickupTruck = 7,
        Truck = 8
    }
}