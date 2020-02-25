using Developing.API.Models.Brands;

namespace Developing.Tests.Factories.Brands
{
    public static class SaveBrandJsonFactory
    {
        public static SaveBrandJson Build(this SaveBrandJson json)
        {
            json.Name = ConstantFactory.Text(length: 30);

            return json;
        }

        public static SaveBrandJson WithName(this SaveBrandJson json, string name)
        {
            json.Name = name;

            return json;
        }
    }
}
