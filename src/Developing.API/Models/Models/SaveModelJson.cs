using System.ComponentModel.DataAnnotations;
using Developing.API.Infrastructure.Database.DataModel.Models;

namespace Developing.API.Models.Models
{
    public sealed class SaveModelJson
    {
        [Required, Range(1, int.MaxValue)]
        public int? BrandId { get; set; }

        [Required, MaxLength(80)]
        public string Name { get; set; }

        public Model MapTo(Model model)
        {
            model.BrandId = BrandId.Value;
            model.Name = Name;

            return model;
        }
    }
}
