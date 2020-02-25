using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Models.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Developing.API.Controllers
{
    [Route("brands")]
    public sealed class BrandsController : Controller
    {
        private readonly ApiDbContext _dbContext;

        public BrandsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> Find([FromRoute] int id)
        {
            var brand = await _dbContext.Brands
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (brand == null)
            {
                return new BrandNotFoundError();
            }

            return new BrandJson(brand);
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> List()
        {
            var brands = await _dbContext.Brands
                .OrderByName()
                .ToListAsync();

            return new BrandListJson(brands);
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Create([FromBody] SaveBrandJson json)
        {
            var nameExists = await _dbContext.Brands
                .WhereNameEqual(json.Name)
                .AnyAsync();

            if (nameExists)
            {
                return new DuplicateBrandName();
            }

            var brand = json.MapTo(new Brand());

            _dbContext.Add(brand);

            await _dbContext.SaveChangesAsync();

            return new BrandJson(brand);
        }

        [HttpPut, Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SaveBrandJson json)
        {
            var brand = await _dbContext.Brands
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (brand == null)
            {
                return new BrandNotFoundError();
            }

            var nameExists = await _dbContext.Brands
                .WhereNameEqual(json.Name)
                .WhereIdNotEqual(id)
                .AnyAsync();

            if (nameExists)
            {
                return new DuplicateBrandName();
            }

            json.MapTo(brand);

            await _dbContext.SaveChangesAsync();

            return new BrandJson(brand);
        }

        [HttpDelete, Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var brand = await _dbContext.Brands
                .WhereId(id)
                .SingleOrDefaultAsync();

            if (brand == null)
            {
                return new BrandNotFoundError();
            }

            var hasModels = await _dbContext.Models
                .WhereBrandId(id)
                .AnyAsync();

            if (hasModels)
            {
                return new BrandHasModelsError();
            }

            _dbContext.Brands.Remove(brand);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
