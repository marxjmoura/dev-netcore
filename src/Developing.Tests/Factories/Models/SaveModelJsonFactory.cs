using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Models.Models;

namespace Developing.Tests.Factories.Models
{
    public static class SaveModelJsonFactory
    {
        public static SaveModelJson To(this SaveModelJson json, Brand brand)
        {
            json.BrandId = brand.Id;
            json.Name = ConstantFactory.Text(length: 30);

            return json;
        }

        public static SaveModelJson WithName(this SaveModelJson json, string name)
        {
            json.Name = name;

            return json;
        }
    }
}
