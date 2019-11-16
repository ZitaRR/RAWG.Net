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

        /// <summary>
        ///     Formats raw html code
        /// </summary>
        /// <param name="text">Html code</param>
        /// <returns>Formatted html code</returns>
        internal static string FormatText(string text)
        {
            if (text is null)
                return string.Empty;
            text = Regex.Replace(text, @"<[^>]*>", "");
            return Regex.Replace(text, @"[\.\,\-\!\?]", (c) => c.NextMatch().Value == " " ? c + " " : c.Value);
        }

        /// <summary>
        ///     Downloads the content of the webpage as a string and 
        ///     deserializes it into the specified class. 
        /// </summary>
        /// <typeparam name="T">Specifies what class it should deserialize to</typeparam>
        /// <param name="query">Endpoint</param>
        /// <returns>The class that was specified with all the data if the request was successful</returns>
        internal async Task<T> SendRequestAsync<T>(string query) where T : Result
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

        /// <summary>
        ///     Gets info about a game
        /// </summary>
        /// <param name="id">ID of the game</param>
        /// <returns>GameResult with info about the game</returns>
        public async Task<GameResult> GetGameAsync(int id)
            => await SendRequestAsync<GameResult>(ENDPOINT + $"games/{id}");

        /// <summary>
        ///     Gets info about a game
        /// </summary>
        /// <param name="slug">Name of the game</param>
        /// <returns>GameResult with info about the game</returns>
        public async Task<GameResult> GetGameAsync(string slug)
        {
            slug = slug.Replace(" ", "-");
            return await SendRequestAsync<GameResult>(ENDPOINT + $"games/{slug}");
        }
    }
}
