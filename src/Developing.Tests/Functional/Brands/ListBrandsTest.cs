using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Models.Brands;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Brands
{
    public sealed class ListBrandsTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiClient _client;

        public ListBrandsTest()
        {
            _server = new FakeApiServer();
            _client = new FakeApiClient(_server);
        }

        [Fact]
        public async Task ShouldListOrderedByName()
        {
            var brand1 = new Brand().WithName("Z Brand");
            var brand2 = new Brand().WithName("A Brand");

            _server.Database.Brands.AddRange(brand1, brand2);
            await _server.Database.SaveChangesAsync();

            var path = "/brands";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<BrandJson[]>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(brand2.Id, jsonResponse.First().Id);
            Assert.Equal(brand1.Id, jsonResponse.Last().Id);
        }
    }
}
