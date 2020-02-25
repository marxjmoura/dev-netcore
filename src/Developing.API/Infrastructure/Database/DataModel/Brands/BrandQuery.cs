using System.Linq;

namespace Developing.API.Infrastructure.Database.DataModel.Brands
{
    public static class BrandQuery
    {
        public static IQueryable<Brand> OrderByName(this IQueryable<Brand> brands)
        {
            return brands.OrderBy(brand => brand.Name);
        }

        public static IQueryable<Brand> WhereId(this IQueryable<Brand> brands, int id)
        {
            return brands.Where(brand => brand.Id == id);
        }

        public static IQueryable<Brand> WhereIdNotEqual(this IQueryable<Brand> brands, int id)
        {
            return brands.Where(brand => brand.Id != id);
        }

        public static IQueryable<Brand> WhereNameEqual(this IQueryable<Brand> brands, string name)
        {
            return brands.Where(brand => brand.Name == name);
        }
    }
}
