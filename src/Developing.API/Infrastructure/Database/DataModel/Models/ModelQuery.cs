using System.Linq;

namespace Developing.API.Infrastructure.Database.DataModel.Models
{
    public static class ModelQuery
    {
        public static IQueryable<Model> OrderByName(this IQueryable<Model> query)
        {
            return query.OrderBy(model => model.Name);
        }

        public static IQueryable<Model> WhereId(this IQueryable<Model> query, int id)
        {
            return query.Where(model => model.Id == id);
        }

        public static IQueryable<Model> WhereBrandId(this IQueryable<Model> query, int? brandId)
        {
            if (brandId == null)
            {
                return query;
            }

            return query.Where(model => model.BrandId == brandId);
        }

        public static IQueryable<Model> WhereIdNotEqual(this IQueryable<Model> query, int id)
        {
            return query.Where(model => model.Id != id);
        }

        public static IQueryable<Model> WhereNameEqual(this IQueryable<Model> query, string name)
        {
            return query.Where(model => model.Name == name);
        }
    }
}
