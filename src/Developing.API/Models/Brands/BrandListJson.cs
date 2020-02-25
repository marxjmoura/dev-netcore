using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Microsoft.AspNetCore.Mvc;

namespace Developing.API.Models.Brands
{
    public class BrandListJson : IActionResult
    {
        private readonly IEnumerable<BrandJson> _brands;

        public BrandListJson(IEnumerable<Brand> brands)
        {
            _brands = brands
                .Select(brand => new BrandJson(brand))
                .ToList();
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(_brands).ExecuteResultAsync(context);
        }
    }
}
