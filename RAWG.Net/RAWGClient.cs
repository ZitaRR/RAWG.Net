using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RAWG.Net
{
    public class RAWGClient
    {
        internal const string ENDPOINT = "https://api.rawg.io/api/";
        private readonly HttpClient client;

        public RAWGClient()
        {
            client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
            client.DefaultRequestHeaders.Add("User-Agent", "RAWG Wrapper");
        }

        internal static string FormatText(string text)
        {
            text = Regex.Replace(text, @"<[^>]*>", "");
            return Regex.Replace(text, @"[\.\,\-\!\?]", (c) => c.NextMatch().Value == " " ? c + " " : c.Value);
        }

        public async Task<dynamic> Test(string query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ENDPOINT + query);
            HttpResponseMessage response = await client.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(content);
        }

        public async Task<T> SendRequestAsync<T>(string query) where T : Result
        {
            var request = new HttpRequestMessage(HttpMethod.Get, query);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new Result().Initialize(response.StatusCode, this) as T;

            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(content);

            try
            {
                bool redirect = data.redirect;
                return await SendRequestAsync<T>(ENDPOINT + $"games/{data.slug}");
            }
            catch
            {
                return JsonConvert.DeserializeObject<T>(content).Initialize(HttpStatusCode.OK, this) as T;
            }
        }

        public async Task<GameResult> GetGameAsync(int id)
            => await SendRequestAsync<GameResult>(ENDPOINT + $"games/{id}");

        public async Task<GameResult> GetGameAsync(string slug)
        {
            slug = slug.Replace(" ", "-");
            return await SendRequestAsync<GameResult>(ENDPOINT + $"games/{slug}");
        }
    }
}
