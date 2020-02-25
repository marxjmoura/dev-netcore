using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Developing.API.Models.Vehicles;
using Developing.Tests.Factories.Brands;
using Developing.Tests.Factories.Models;
using Developing.Tests.Factories.Vehicles;
using Developing.Tests.Fakes;
using Xunit;

namespace Developing.Tests.Functional.Vehicles
{
    public sealed class ListVehiclesTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiClient _client;

        public ListVehiclesTest()
        {
            _server = new FakeApiServer();
            _client = new FakeApiClient(_server);
        }

        [Fact]
        public async Task ShouldListOrderedByValue()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);
            var vehicle1 = new Vehicle().To(model).WithValue(12000);
            var vehicle2 = new Vehicle().To(model).WithValue(11000);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);
            _server.Database.Vehicles.AddRange(vehicle1, vehicle2);

            await _server.Database.SaveChangesAsync();

            var path = "/vehicles";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<VehicleJson[]>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(vehicle2.Id, jsonResponse.First().Id);
            Assert.Equal(vehicle1.Id, jsonResponse.Last().Id);
        }

        [Fact]
        public async Task ShouldListByModel()
        {
            var brand = new Brand().Build();
            var model1 = new Model().To(brand);
            var model2 = new Model().To(brand);
            var vehicle1 = new Vehicle().To(model1);
            var vehicle2 = new Vehicle().To(model2);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.AddRange(model1, model2);
            _server.Database.Vehicles.AddRange(vehicle1, vehicle2);

            await _server.Database.SaveChangesAsync();

            var path = $"/vehicles?modelId={model1.Id}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadAsJsonAsync<VehicleJson[]>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(jsonResponse, json => json.Model == model1.Name);
            Assert.DoesNotContain(jsonResponse, json => json.Model == model2.Name);
        }
    }
}
