using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace Developing.API.Models.Vehicles
{
    public sealed class VehicleListJson : IActionResult
    {
        private readonly IEnumerable<VehicleJson> _vehicles;

        public VehicleListJson(IEnumerable<Vehicle> vehicles)
        {
            _vehicles = vehicles
                .Select(vehicle => new VehicleJson(vehicle))
                .ToList();
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(_vehicles).ExecuteResultAsync(context);
        }
    }
}
