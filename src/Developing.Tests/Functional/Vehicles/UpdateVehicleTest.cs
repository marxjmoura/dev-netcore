using System.Net;
using System.Threading.Tasks;
using Developing.API.Authorization;
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
    public sealed class UpdateVehicleTest
    {
        private readonly FakeApiServer _server;

        public UpdateVehicleTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldUpdateDatabase()
        {
            var brand = new Brand().Build();
            var model1 = new Model().To(brand);
            var model2 = new Model().To(brand);
            var vehicle = new Vehicle().To(model1);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.AddRange(model1, model2);
            _server.Database.Vehicles.Add(vehicle);

            await _server.Database.SaveChangesAsync();

            var path = $"/vehicles/{vehicle.Id}";
            var jsonRequest = new SaveVehicleJson().To(model2);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<VehicleJson>(response);

            await _server.Database.Entry(vehicle).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(jsonRequest.ModelId, vehicle.ModelId);
            Assert.Equal(jsonRequest.ModelYear, vehicle.ModelYear);
            Assert.Equal(jsonRequest.Fuel, vehicle.Fuel);
            Assert.Equal(jsonRequest.Value, vehicle.Value);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentModelId()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);
            var unsavedModel = new Model { Id = 9 };
            var vehicle = new Vehicle().To(model);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);
            _server.Database.Vehicles.Add(vehicle);

            await _server.Database.SaveChangesAsync();

            var path = $"/vehicles/{vehicle.Id}";
            var jsonRequest = new SaveVehicleJson().To(unsavedModel);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("MODEL_NOT_FOUND", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = "/vehicles/1";
            var jsonRequest = new SaveVehicleJson().To(model);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("VEHICLE_NOT_FOUND", jsonResponse.Error);
        }
    }
}
