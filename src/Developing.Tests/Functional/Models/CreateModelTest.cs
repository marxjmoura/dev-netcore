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
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Developing.Tests.Functional.Models
{
    public sealed class CreateModelTest
    {
        private readonly FakeApiServer _server;

        public CreateModelTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldSaveToDatabase()
        {
            var brand = new Brand().Build();

            _server.Database.Brands.Add(brand);
            await _server.Database.SaveChangesAsync();

            var path = "/models";
            var jsonRequest = new SaveModelJson().To(brand);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<ModelJson>(response);
            var model = await _server.Database.Models.SingleAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(jsonRequest.Name, model.Name);
        }

        [Fact]
        public async Task ShouldRespond422ForDuplicateName()
        {
            var brand = new Brand().Build();
            var model = new Model().To(brand);

            _server.Database.Brands.Add(brand);
            _server.Database.Models.Add(model);

            await _server.Database.SaveChangesAsync();

            var path = "/models";
            var jsonRequest = new SaveModelJson().To(brand).WithName(model.Name);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("DUPLICATE_MODEL_NAME", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond404ForInexistentBrandId()
        {
            var unsavedBrand = new Brand { Id = 1 };

            var path = "/models";
            var jsonRequest = new SaveModelJson().To(unsavedBrand);
            var token = new ApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);
            var response = await client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadAsJsonAsync<NotFoundError>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("BRAND_NOT_FOUND", jsonResponse.Error);
        }
    }
}
