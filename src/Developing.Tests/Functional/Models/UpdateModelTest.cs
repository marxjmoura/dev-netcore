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
    public sealed class UpdateModelTest
    {
        private readonly FakeApiServer _server;

        public UpdateModelTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldUpdateDatabase()
        {
            var brand1 = new Brand().Build();
            var brand2 = new Brand().Build();
            var model = new Model().To(brand1);

            _server.Database.Brands.AddRange(brand1, brand2);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = $"/models/{model.Id}";
            var jsonRequest = new SaveModelJson().To(brand2);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<ModelJson>(response);

            await _server.Database.Entry(model).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(jsonRequest.Name, model.Name);
        }

        [Fact]
        public async Task ShouldRespond422ForDuplicateName()
        {
            var brand = new Brand().Build();
            var model1 = new Model().To(brand);
            var model2 = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.AddRange(model1, model2);

            await _server.Database.SaveChangesAsync();

            var path = $"/models/{model1.Id}";
            var jsonRequest = new SaveModelJson().To(brand).WithName(model2.Name);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("DUPLICATE_MODEL_NAME", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentBrandId()
        {
            var brand = new Brand().Build();
            var unsavedBrand = new Brand { Id = 9 };
            var model = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = $"/models/{model.Id}";
            var jsonRequest = new SaveModelJson().To(unsavedBrand);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("BRAND_NOT_FOUND", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentId()
        {
            var brand = new Brand().Build();

            _server.Database.Brands.Add(brand);
            await _server.Database.SaveChangesAsync();

            var path = "/models/1";
            var jsonRequest = new SaveModelJson().To(brand);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("MODEL_NOT_FOUND", jsonResponse.Error);
        }
    }
}
