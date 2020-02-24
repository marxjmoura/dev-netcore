using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Models.Models;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Factories.Models;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Models
{
    public sealed class ListModelTest
    {
        private readonly FakeApiServer _server;

        public ListModelTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldListOrderedByName()
        {
            var brand = new Brand().Build();
            var model1 = new Model().To(brand).WithName("Z Model");
            var model2 = new Model().To(brand).WithName("A Model");

            _server.Database.Brands.Add(brand);
            _server.Database.Models.AddRange(model1, model2);

            await _server.Database.SaveChangesAsync();

            var path = "/models";
            var client = new FakeApiClient(_server);
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<ModelJson[]>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(model2.Id, jsonResponse.First().Id);
            Assert.Equal(model1.Id, jsonResponse.Last().Id);
        }

        [Fact]
        public async Task ShouldListByBrand()
        {
            var brand1 = new Brand().Build();
            var brand2 = new Brand().Build();
            var model1 = new Model().To(brand1);
            var model2 = new Model().To(brand2);

            _server.Database.Brands.AddRange(brand1, brand2);
            _server.Database.Models.AddRange(model1, model2);

            await _server.Database.SaveChangesAsync();

            var path = $"/models?brandId={brand1.Id}";
            var client = new FakeApiClient(_server);
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadAsJsonAsync<ModelJson[]>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(jsonResponse, json => json.Brand == brand1.Name);
            Assert.DoesNotContain(jsonResponse, json => json.Brand == brand2.Name);
        }
    }
}
