using System.Net;
using System.Threading.Tasks;
using Developing.API.Authorization;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Models;
using Developing.API.Models.Brands;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Brands
{
    public sealed class FindBrandTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiClient _client;

        public FindBrandTest()
        {
            _server = new FakeApiServer();
            _client = new FakeApiClient(_server, new ApiToken(_server.JwtOptions));
        }

        [Fact]
        public async Task ShouldFind()
        {
            var brand = new Brand().Build();

            _server.Database.Brands.Add(brand);
            await _server.Database.SaveChangesAsync();

            var path = $"/brands/{brand.Id}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<BrandJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(brand.Id, jsonResponse.Id);
            Assert.Equal(brand.Name, jsonResponse.Name);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var path = "/brands/1";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("BRAND_NOT_FOUND", jsonResponse.Error);
        }
    }
}
