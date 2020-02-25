using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Developing.API.Authorization;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace Developing.Tests.Fakes
{
    public sealed class FakeApiClient
    {
        private readonly HttpClient _client;

        public FakeApiClient(TestServer server, ApiToken token = null)
        {
            _client = server.CreateClient();

            if (token != null)
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.ToString());
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await _client.DeleteAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostJsonAsync(string requestUri, object content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            return await PostJsonAsync(requestUri, jsonContent);
        }

        public async Task<HttpResponseMessage> PostJsonAsync(string requestUri, string content)
        {
            var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await _client.PostAsync(requestUri, jsonContent);
        }

        public async Task<HttpResponseMessage> PutJsonAsync(string requestUri, object content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            return await PutJsonAsync(requestUri, jsonContent);
        }

        public async Task<HttpResponseMessage> PutJsonAsync(string requestUri, string content)
        {
            var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await _client.PutAsync(requestUri, jsonContent);
        }

        public async Task<TResult> ReadAsJsonAsync<TResult>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(json);
        }
    }
}
