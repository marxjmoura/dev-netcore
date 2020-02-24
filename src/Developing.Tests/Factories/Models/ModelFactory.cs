using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;

namespace Developing.Tests.Factories.Models
{
    public static class ModelFactory
    {
        public static Model To(this Model model, Brand brand)
        {
            model.Name = ConstantFactory.Text(length: 30);
            model.Brand = brand;

            return model;
        }

        public static Model WithName(this Model model, string name)
        {
            model.Name = name;

            return model;
        }
    }
}
