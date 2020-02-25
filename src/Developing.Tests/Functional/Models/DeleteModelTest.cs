using System.Net;
using System.Threading.Tasks;
using Developing.API.Authorization;
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

namespace Developing.Tests.Functional.Models
{
    public sealed class DeleteModelTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiClient _client;

        public DeleteModelTest()
        {
            _server = new FakeApiServer();
            _client = new FakeApiClient(_server, new ApiToken(_server.JwtOptions));
        }

        [Fact]
        public async Task ShouldDeleteFromDatabase()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = $"/models/{model.Id}";
            var response = await _client.DeleteAsync(path);
            var hasBeenDeleted = !await _server.Database.Models
                .WhereId(model.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task ShouldRespond422WhenHasVehicles()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);
            var vehicle = new Vehicle().To(model);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);
            _server.Database.Vehicles.Add(vehicle);

            await _server.Database.SaveChangesAsync();

            var path = $"/models/{model.Id}";
            var response = await _client.DeleteAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("MODEL_HAS_VEHICLES", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var path = "/models/1";
            var response = await _client.DeleteAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("MODEL_NOT_FOUND", jsonResponse.Error);
        }
    }
}
