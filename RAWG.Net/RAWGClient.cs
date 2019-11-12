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
            string content = await response.Content.ReadAsStringAsync();
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                string error = data.detail.ToString();
                return new Result(null, null, error);
            }
            catch
            {
                return JsonConvert.DeserializeObject<Result>(content);
            }
        }

        public async Task<Result> GetGame(int id)
        {
            var response = await SendRequestAsync("https://api.rawg.io/api/games/" + id);
            return new Result(response.Name, response.ID);
        }

        public async Task<Result> GetGame(string slug)
        {
            var response = await SendRequestAsync("https://api.rawg.io/api/games/" + slug);
            return new Result(response.Name, response.ID);
        }
    }
}
