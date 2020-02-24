using System.Linq;

namespace Developing.API.Infrastructure.Database.DataModel.Brands
{
    public static class BrandQuery
    {
        public static IQueryable<Brand> OrderByName(this IQueryable<Brand> query)
        {
            return query.OrderBy(brand => brand.Name);
        }

        public static IQueryable<Brand> WhereId(this IQueryable<Brand> query, int id)
        {
            return query.Where(brand => brand.Id == id);
        }

        public static IQueryable<Brand> WhereIdNotEqual(this IQueryable<Brand> query, int id)
        {
            return query.Where(brand => brand.Id != id);
        }

        public static IQueryable<Brand> WhereNameEqual(this IQueryable<Brand> query, string name)
        {
            return query.Where(brand => brand.Name == name);
        }
    }
}
