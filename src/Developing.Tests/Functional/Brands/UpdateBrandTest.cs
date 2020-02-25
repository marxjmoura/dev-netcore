using System.Net;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Models;
using Developing.API.Models.Brands;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Brands
{
    public sealed class UpdateBrandTest
    {
        private readonly FakeApiServer _server;

        public UpdateBrandTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldUpdateDatabase()
        {
            var brand = new Brand().Build();

            _server.Database.Brands.Add(brand);
            await _server.Database.SaveChangesAsync();

            var path = $"/brands/{brand.Id}";
            var jsonRequest = new SaveBrandJson().Build();
            var client = new FakeApiClient(_server);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<BrandJson>(response);

            await _server.Database.Entry(brand).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(jsonRequest.Name, brand.Name);
        }

        [Fact]
        public async Task ShouldRespond422ForDuplicateName()
        {
            var brand1 = new Brand().Build();
            var brand2 = new Brand().Build();

            _server.Database.Brands.AddRange(brand1, brand2);
            await _server.Database.SaveChangesAsync();

            var path = $"/brands/{brand1.Id}";
            var jsonRequest = new SaveBrandJson().WithName(brand2.Name);
            var client = new FakeApiClient(_server);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("DUPLICATE_BRAND_NAME", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var path = "/brands/1";
            var jsonRequest = new SaveBrandJson().Build();
            var client = new FakeApiClient(_server);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("BRAND_NOT_FOUND", jsonResponse.Error);
        }
    }
}
