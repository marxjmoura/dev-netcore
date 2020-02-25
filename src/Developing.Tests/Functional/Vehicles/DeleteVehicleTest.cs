using System.Net;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Developing.API.Models;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Factories.Models;
using Developing.Tests.Factories.Vehicles;
using Developing.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Developing.Tests.Functional.Vehicles
{
    public sealed class DeleteVehicleTest
    {
        private readonly FakeApiServer _server;

        public DeleteVehicleTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldDeleteFromDatabase()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);
            var vehicle = new Vehicle().To(model);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);
            _server.Database.Vehicles.Add(vehicle);

            await _server.Database.SaveChangesAsync();

            var path = $"/vehicles/{vehicle.Id}";
            var client = new FakeApiClient(_server);
            var response = await client.DeleteAsync(path);
            var hasBeenDeleted = !await _server.Database.Vehicles
                .WhereId(vehicle.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var path = "/vehicles/1";
            var client = new FakeApiClient(_server);
            var response = await client.DeleteAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("VEHICLE_NOT_FOUND", jsonResponse.Error);
        }
    }
}
