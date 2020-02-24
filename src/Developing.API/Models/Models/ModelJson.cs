using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Developing.API.Models.Models
{
    public sealed class ModelJson : IActionResult
    {
        public ModelJson() { }

        public ModelJson(Model model)
        {
            Id = model.Id;
            Name = model.Name;
            Brand = model.Brand.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
