using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Microsoft.AspNetCore.Mvc;

namespace Developing.API.Models.Brands
{
    public sealed class BrandJson : IActionResult
    {
        public BrandJson() { }

        public BrandJson(Brand brand)
        {
            Id = brand.Id;
            Name = brand.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
