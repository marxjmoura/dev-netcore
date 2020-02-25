using System.Net;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Developing.API.Models;
using Developing.API.Models.Vehicles;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Factories.Models;
using Developing.Tests.Factories.Vehicles;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Vehicles
{
    public sealed class FindVehicleTest
    {
        private readonly FakeApiServer _server;

        public FindVehicleTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldFind()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);
            var vehicle = new Vehicle().To(model).WithValue(11532);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);
            _server.Database.Vehicles.Add(vehicle);

            await _server.Database.SaveChangesAsync();

            var path = $"/vehicles/{vehicle.Id}";
            var client = new FakeApiClient(_server);
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<VehicleJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(brand.Name, jsonResponse.Brand);
            Assert.Equal(model.Name, jsonResponse.Model);
            Assert.Equal(vehicle.ModelYear, jsonResponse.ModelYear);
            Assert.Equal(vehicle.Fuel, jsonResponse.Fuel);
            Assert.Equal("R$ 11.532,00", jsonResponse.Value);
            Assert.Equal(11532, jsonResponse.RawValue);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var path = "/vehicles/1";
            var client = new FakeApiClient(_server);
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("VEHICLE_NOT_FOUND", jsonResponse.Error);
        }
    }
}
