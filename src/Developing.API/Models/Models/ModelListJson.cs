using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Developing.API.Models.Models
{
    public sealed class ModelListJson : IActionResult
    {
        private readonly IEnumerable<ModelJson> _models;

        public ModelListJson() { }

        public ModelListJson(IEnumerable<Model> models)
        {
            _models = models
                .Select(model => new ModelJson(model))
                .ToList();
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(_models).ExecuteResultAsync(context);
        }
    }
}
