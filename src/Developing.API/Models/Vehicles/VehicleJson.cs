using System.Globalization;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace Developing.API.Models.Vehicles
{
    public sealed class VehicleJson : IActionResult
    {
        public VehicleJson() { }

        public VehicleJson(Vehicle vehicle)
        {
            Id = vehicle.Id;
            ModelYear = vehicle.ModelYear;
            Fuel = vehicle.Fuel;
            Brand = vehicle.Model.Brand.Name;
            Model = vehicle.Model.Name;
            RawValue = vehicle.Value;

            Value = vehicle.Value.ToString("R$ #,##0.00", new NumberFormatInfo
            {
                NumberGroupSeparator = ".",
                NumberDecimalSeparator = ","
            });
        }

        public int Id { get; set; }
        public int ModelYear { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Fuel { get; set; }
        public string Value { get; set; }
        public decimal RawValue { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
