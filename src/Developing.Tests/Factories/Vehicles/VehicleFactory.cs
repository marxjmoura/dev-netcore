using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;

namespace Developing.Tests.Factories.Vehicles
{
    public static class VehicleFactory
    {
        public static Vehicle To(this Vehicle json, Model model)
        {
            json.Model = model;
            json.ModelYear = 2015;
            json.Fuel = "Gasolina";
            json.Value = 15000;

            return json;
        }

        public static Vehicle WithValue(this Vehicle json, decimal value)
        {
            json.Value = value;

            return json;
        }
    }
}
