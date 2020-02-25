using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Developing.API.Models.Brands;
using Developing.API.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Developing.API.Controllers
{
    [Route("models")]
    public sealed class ModelsController : Controller
    {
        private readonly ApiDbContext _dbContext;

        public ModelsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> Find([FromRoute] int id)
        {
            var model = await _dbContext.Models
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return new ModelNotFoundError();
            }

            return new ModelJson(model);
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> List([FromQuery] ModelListQuery query)
        {
            var models = await _dbContext.Models
                .WhereBrandId(query.BrandId)
                .OrderByName()
                .ToListAsync();

            return new ModelListJson(models);
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Create([FromBody] SaveModelJson json)
        {
            var nameExists = await _dbContext.Models
                .WhereNameEqual(json.Name)
                .AnyAsync();

            if (nameExists)
            {
                return new DuplicateModelName();
            }

            var brandExists = await _dbContext.Brands
                .WhereId(json.BrandId.Value)
                .AnyAsync();

            if (!brandExists)
            {
                return new BrandNotFoundError();
            }

            var model = json.MapTo(new Model());

            _dbContext.Add(model);

            await _dbContext.SaveChangesAsync();

            return new ModelJson(model);
        }

        [HttpPut, Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SaveModelJson json)
        {
            var model = await _dbContext.Models
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return new ModelNotFoundError();
            }

            var nameExists = await _dbContext.Models
                .WhereNameEqual(json.Name)
                .WhereIdNotEqual(id)
                .AnyAsync();

            if (nameExists)
            {
                return new DuplicateModelName();
            }

            var brandExists = await _dbContext.Brands
                .WhereId(json.BrandId.Value)
                .AnyAsync();

            if (!brandExists)
            {
                return new BrandNotFoundError();
            }

            json.MapTo(model);

            await _dbContext.SaveChangesAsync();

            return new ModelJson(model);
        }

        [HttpDelete, Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var model = await _dbContext.Models
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return new ModelNotFoundError();
            }

            var hasVehicles = await _dbContext.Vehicles
                .WhereModelId(id)
                .AnyAsync();

            if (hasVehicles)
            {
                return new ModelHasVehiclesError();
            }

            _dbContext.Models.Remove(model);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
