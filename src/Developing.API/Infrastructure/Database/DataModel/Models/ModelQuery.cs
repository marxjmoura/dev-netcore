using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Developing.API.Infrastructure.Database.DataModel.Models
{
    public static class ModelQuery
    {
        public static IQueryable<Model> IncludeBrand(this IQueryable<Model> models)
        {
            return models.Include(model => model.Brand);
        }

        public static IQueryable<Model> OrderByName(this IQueryable<Model> models)
        {
            return models.OrderBy(model => model.Name);
        }

        public static IQueryable<Model> WhereId(this IQueryable<Model> models, int id)
        {
            return models.Where(model => model.Id == id);
        }

        public static IQueryable<Model> WhereBrandId(this IQueryable<Model> models, int? brandId)
        {
            if (brandId == null)
            {
                return models;
            }

            return models.Where(model => model.BrandId == brandId);
        }

        public static IQueryable<Model> WhereIdNotEqual(this IQueryable<Model> models, int id)
        {
            return models.Where(model => model.Id != id);
        }

        public static IQueryable<Model> WhereNameEqual(this IQueryable<Model> models, string name)
        {
            return models.Where(model => model.Name == name);
        }
    }
}
