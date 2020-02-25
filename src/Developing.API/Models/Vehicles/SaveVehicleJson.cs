using System.ComponentModel.DataAnnotations;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;

namespace Developing.API.Models.Vehicles
{
    public sealed class SaveVehicleJson
    {
        [Required, Range(1, int.MaxValue)]
        public int? ModelId { get; set; }

        [Required, Range(1900, 9999)]
        public int? ModelYear { get; set; }

        [Required, MaxLength(30)]
        public string Fuel { get; set; }

        [Required, Range(0, 999999.99)]
        public decimal? Value { get; set; }

        public Vehicle MapTo(Vehicle vehicle)
        {
            vehicle.ModelId = ModelId.Value;
            vehicle.ModelYear = ModelYear.Value;
            vehicle.Fuel = Fuel;
            vehicle.Value = Value.Value;

            return vehicle;
        }
    }
}
