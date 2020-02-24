using Developing.API.Infrastructure.Database.DataModel;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Developing.Tests.Fakes
{
    public sealed class FakeApiServer : TestServer
    {
        public FakeApiServer() : base(new Program().CreateWebHostBuilder()) { }

        public ApiDbContext Database => Host.Services.GetService<ApiDbContext>();
    }
}
