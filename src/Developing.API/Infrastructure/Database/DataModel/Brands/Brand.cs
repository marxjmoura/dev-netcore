using System.Collections.Generic;
using Developing.API.Infrastructure.Database.DataModel.Models;

namespace Developing.API.Infrastructure.Database.DataModel.Brands
{
    public sealed class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Model> Models { get; set; }
    }
}
