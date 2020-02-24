using Developing.API.Infrastructure.Database.DataModel.Models;

namespace Developing.API.Infrastructure.Database.DataModel.Vehicles
{
    public sealed class Vehicle
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int ModelYear { get; set; }
        public string Fuel { get; set; }
        public decimal Value { get; set; }
        public Model Model { get; set; }
    }
}
