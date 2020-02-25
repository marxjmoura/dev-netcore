using System.Net;
using System.Threading.Tasks;
using Developing.API.Authorization;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Models;
using Developing.API.Models.Models;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Factories.Models;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Models
{
    public sealed class FindModelTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiClient _client;

        public FindModelTest()
        {
            _server = new FakeApiServer();
            _client = new FakeApiClient(_server, new ApiToken(_server.JwtOptions));
        }

        [Fact]
        public async Task ShouldFind()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = $"/models/{model.Id}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<ModelJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(model.Id, jsonResponse.Id);
            Assert.Equal(model.Name, jsonResponse.Name);
            Assert.Equal(brand.Name, jsonResponse.Brand);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var path = "/models/1";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("MODEL_NOT_FOUND", jsonResponse.Error);
        }
    }
}
