using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Models.Vehicles;

namespace Developing.Tests.Factories.Vehicles
{
    public static class SaveVehicleJsonFactory
    {
        public static SaveVehicleJson To(this SaveVehicleJson json, Model model)
        {
            json.ModelId = model.Id;
            json.ModelYear = 2015;
            json.Fuel = "Gasolina";
            json.Value = 12500;

            return json;
        }
    }
}
