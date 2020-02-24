using Developing.API.Infrastructure.Database.DataModel.Brands;

namespace Developing.Tests.Factories.Brands
{
    public static class BrandFactory
    {
        public static Brand Build(this Brand brand)
        {
            brand.Name = ConstantFactory.Text(length: 30);

            return brand;
        }
    }
}
