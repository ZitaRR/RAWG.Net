using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RAWG.Net
{
    public class RAWGClient
    {
        internal const string endpoint = "https://api.rawg.io/api/";
        private readonly HttpClient client;

        public RAWGClient()
        {
            client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
            client.DefaultRequestHeaders.Add("User-Agent", "RAWG Wrapper");
        }

        private async Task<T> SendRequestAsync<T>(string query) where T : Result
        {
            var request = new HttpRequestMessage(HttpMethod.Get, query);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new Result().Initialize(response.StatusCode) as T;

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content).Initialize(HttpStatusCode.OK) as T;
        }

        public async Task<GameResult> GetGame(int id)
            => await SendRequestAsync<GameResult>(endpoint + $"games/{id}");

        public async Task<GameResult> GetGame(string slug)
        {
            slug = slug.Replace(" ", "-");
            return await SendRequestAsync<GameResult>(endpoint + $"games/{slug}");
        }
    }
}
