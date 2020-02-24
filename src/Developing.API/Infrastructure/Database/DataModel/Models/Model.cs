using System.Collections.Generic;
using Developing.API.Infrastructure.Database.DataModel.Brands;

namespace Developing.API.Infrastructure.Database.DataModel.Models
{
    public sealed class Model
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
    }
}
