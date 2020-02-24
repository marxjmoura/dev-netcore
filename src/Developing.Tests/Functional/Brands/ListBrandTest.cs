using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Models.Brands;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Brands
{
    public sealed class ListBrandTest
    {
        private readonly FakeApiServer _server;

        public ListBrandTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldListOrderedByName()
        {
            var brand1 = new Brand { Name = "Z Brand" };
            var brand2 = new Brand { Name = "A Brand" };

            _server.Database.Brands.AddRange(brand1, brand2);
            await _server.Database.SaveChangesAsync();

            var path = "/brands";
            var client = new FakeApiClient(_server);
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<BrandJson[]>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(brand2.Id, jsonResponse.First().Id);
            Assert.Equal(brand1.Id, jsonResponse.Last().Id);
        }
    }
}
