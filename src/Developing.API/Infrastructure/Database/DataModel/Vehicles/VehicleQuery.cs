using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Developing.API.Infrastructure.Database.DataModel.Vehicles
{
    public static class VehicleQuery
    {
        public static IQueryable<Vehicle> IncludeModel(this IQueryable<Vehicle> vehicles)
        {
            return vehicles.Include(vehicle => vehicle.Model);
        }

        public static IQueryable<Vehicle> IncludeBrand(this IQueryable<Vehicle> vehicles)
        {
            return vehicles.Include(vehicle => vehicle.Model)
                .ThenInclude(model => model.Brand);
        }

        public static IQueryable<Vehicle> OrderByValue(this IQueryable<Vehicle> vehicles)
        {
            return vehicles.OrderBy(vehicle => vehicle.Value);
        }

        public static IQueryable<Vehicle> WhereId(this IQueryable<Vehicle> vehicles, int id)
        {
            return vehicles.Where(vehicle => vehicle.Id == id);
        }

        public static IQueryable<Vehicle> WhereModelId(this IQueryable<Vehicle> vehicles, int? modelId)
        {
            if (modelId == null)
            {
                return vehicles;
            }

            return vehicles.Where(vehicle => vehicle.ModelId == modelId);
        }
    }
}
