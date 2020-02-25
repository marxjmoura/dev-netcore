using System.Net;
using System.Threading.Tasks;
using Developing.API.Authorization;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Models;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Factories.Models;
using Developing.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Developing.Tests.Functional.Brands
{
    public sealed class DeleteBrandTest
    {
        private readonly FakeApiServer _server;

        public DeleteBrandTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldDeleteFromDatabase()
        {
            var brand = new Brand().Build();

            _server.Database.Brands.Add(brand);
            await _server.Database.SaveChangesAsync();

            var path = $"/brands/{brand.Id}";
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.DeleteAsync(path);
            var hasBeenDeleted = !await _server.Database.Brands
                .WhereId(brand.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task ShouldRespond422WhenHasModels()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = $"/brands/{brand.Id}";
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.DeleteAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("BRAND_HAS_MODELS", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var path = "/brands/1";
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.DeleteAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("BRAND_NOT_FOUND", jsonResponse.Error);
        }
    }
}
