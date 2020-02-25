using System.Net;
using System.Threading.Tasks;
using Developing.API.Authorization;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Models;
using Developing.API.Models.Brands;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Developing.Tests.Functional.Brands
{
    public sealed class CreateBrandTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiClient _client;

        public CreateBrandTest()
        {
            _server = new FakeApiServer();
            _client = new FakeApiClient(_server, new ApiToken(_server.JwtOptions));
        }

        [Fact]
        public async Task ShouldSaveToDatabase()
        {
            var path = "/brands";
            var jsonRequest = new SaveBrandJson().Build();
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadAsJsonAsync<BrandJson>(response);
            var brand = await _server.Database.Brands.SingleAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(jsonRequest.Name, brand.Name);
        }

        [Fact]
        public async Task ShouldRespond422ForDuplicateName()
        {
            var brand = new Brand().Build();

            _server.Database.Brands.Add(brand);
            await _server.Database.SaveChangesAsync();

            var path = "/brands";
            var jsonRequest = new SaveBrandJson().WithName(brand.Name);
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadAsJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("DUPLICATE_BRAND_NAME", jsonResponse.Error);
        }
    }
}
