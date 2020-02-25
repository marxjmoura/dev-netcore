using System.Net;
using System.Threading.Tasks;
using Developing.API.Authorization;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Models;
using Developing.API.Models.Vehicles;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Factories.Models;
using Developing.Tests.Factories.Vehicles;
using Developing.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Developing.Tests.Functional.Vehicles
{
    public sealed class CreateVehicleTest
    {
        private readonly FakeApiServer _server;

        public CreateVehicleTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldSaveToDatabase()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = "/vehicles";
            var jsonRequest = new SaveVehicleJson().To(model);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<VehicleJson>(response);
            var vehicle = await _server.Database.Vehicles.SingleAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(jsonRequest.ModelId, vehicle.ModelId);
            Assert.Equal(jsonRequest.ModelYear, vehicle.ModelYear);
            Assert.Equal(jsonRequest.Fuel, vehicle.Fuel);
            Assert.Equal(jsonRequest.Value, vehicle.Value);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentBrandId()
        {
            var unsavedModel = new Model { Id = 1 };

            var path = "/vehicles";
            var jsonRequest = new SaveVehicleJson().To(unsavedModel);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("MODEL_NOT_FOUND", jsonResponse.Error);
        }
    }
}
