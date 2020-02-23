using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Developing.API.Models
{
    public class NotFoundError : IActionResult
    {
        public string Error { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var json = new JsonResult(this) { StatusCode = 404 };

            await json.ExecuteResultAsync(context);
        }
    }
}
