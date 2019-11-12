using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RAWG.Net
{
    public class RAWGClient
    {
        private readonly HttpClient client;

        public RAWGClient()
        {
            client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
            client.DefaultRequestHeaders.Add("User-Agent", "RAWG Wrapper");
        }

        private async Task<Result> SendRequestAsync(string query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, query);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new Result(null, null).Initialize(response.StatusCode);

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Result>(content).Initialize(HttpStatusCode.OK);
        }

        public async Task<Result> GetGame(int id)
            => await SendRequestAsync("https://api.rawg.io/api/games/" + id);

        public async Task<Result> GetGame(string slug)
        {
            slug = slug.Replace(" ", "-");
            return await SendRequestAsync("https://api.rawg.io/api/games/" + slug);
        }
    }
}
