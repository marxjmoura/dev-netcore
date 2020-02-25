using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Developing.API.Models.Models;
using Developing.API.Models.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Developing.API.Controllers
{
    [Route("vehicles")]
    public sealed class VehiclesController : Controller
    {
        private readonly ApiDbContext _dbContext;

        public VehiclesController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> Find([FromRoute] int id)
        {
            var vehicle = await _dbContext.Vehicles
                .WhereId(id)
                .IncludeModel()
                .IncludeBrand()
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (vehicle == null)
            {
                return new VehicleNotFoundError();
            }

            return new VehicleJson(vehicle);
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> List([FromQuery] VehicleListQuery query)
        {
            var vehicles = await _dbContext.Vehicles
                .WhereModelId(query.ModelId)
                .OrderByValue()
                .IncludeModel()
                .IncludeBrand()
                .AsNoTracking()
                .ToListAsync();

            return new VehicleListJson(vehicles);
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Create([FromBody] SaveVehicleJson json)
        {
            var modelExists = await _dbContext.Models
                .WhereId(json.ModelId.Value)
                .AnyAsync();

            if (!modelExists)
            {
                return new ModelNotFoundError();
            }

            var vehicle = json.MapTo(new Vehicle());

            _dbContext.Add(vehicle);

            await _dbContext.SaveChangesAsync();

            return new VehicleJson(vehicle);
        }

        [HttpPut, Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SaveVehicleJson json)
        {
            var vehicle = await _dbContext.Vehicles
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (vehicle == null)
            {
                return new VehicleNotFoundError();
            }

            var modelExists = await _dbContext.Models
                .WhereId(json.ModelId.Value)
                .AnyAsync();

            if (!modelExists)
            {
                return new ModelNotFoundError();
            }

            json.MapTo(vehicle);

            await _dbContext.SaveChangesAsync();

            return new VehicleJson(vehicle);
        }

        [HttpDelete, Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var vehicle = await _dbContext.Vehicles
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (vehicle == null)
            {
                return new VehicleNotFoundError();
            }

            _dbContext.Vehicles.Remove(vehicle);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
