using System.ComponentModel.DataAnnotations;
using Developing.API.Infrastructure.Database.DataModel.Brands;

namespace Developing.API.Models.Brands
{
    public sealed class SaveBrandJson
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }

        public Brand MapTo(Brand brand)
        {
            brand.Name = Name;

            return brand;
        }
    }
}
