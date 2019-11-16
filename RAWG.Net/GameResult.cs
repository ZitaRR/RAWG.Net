using System;
using System.Threading.Tasks;

namespace RAWG.Net
{
    public class GameResult : Result
    {
        public string Name { get; }
        public int? ID { get; }
        public string Description { get; }
        public int? MetaCritic { get; }
        public DateTime? Released { get; }
        public string ImageURI { get; }
        public string Website { get; }
        public Rating UserRating { get; }
        public string ESRB { get; }
        public int? DeveloperID { get; }
        public int? PublisherID { get; }

        public GameResult(string name = null, int? id = null, string description = "", int? metaCritic = null,
            string released = null, string background_image = null, string website = null, dynamic esrb_rating = null,
            Rating[] ratings = null, dynamic[] developers = null, dynamic[] publishers = null)
        {
            Name = name;
            ID = id;
            Description = RAWGClient.FormatText(description);
            MetaCritic = metaCritic;
            Released = DateTime.TryParse(released, out DateTime time) ? time as DateTime? : null;
            ImageURI = background_image;
            Website = website;
            UserRating = ratings is null || ratings.Length == 0 ? null : ratings[0];
            ESRB = esrb_rating?.name ?? "N/A";
            DeveloperID = developers[0].id;
            PublisherID = publishers[0].id;
        }

        /// <summary>
        ///     Gets all the DLC's for ths game
        /// </summary>
        /// <returns>An array of DLC's for this game</returns>
        public async Task<DLCResult[]> GetDLCsAsync()
        {
            var dlcs = await client.SendRequestAsync<DLCResult>(RAWGClient.ENDPOINT + $"games/{ID}/additions");
            return dlcs.Initialize();
        }

        /// <summary>
        ///     Gets all the trailers for this game
        /// </summary>
        /// <returns>An array of trailers for this game</returns>
        public async Task<TrailerResult[]> GetTrailersAsync()
        {
            var trailers = await client.SendRequestAsync<TrailerResult>(RAWGClient.ENDPOINT + $"games/{ID}/movies");
            return trailers.Initialize();
        }

        /// <summary>
        ///     Gets the developer for this game
        /// </summary>
        /// <returns>DeveloperResult with info about the developer</returns>
        public async Task<DeveloperResult> GetDeveloperAsync()
            => await client.SendRequestAsync<DeveloperResult>(RAWGClient.ENDPOINT + $"developers/{DeveloperID}");

        /// <summary>
        ///     Gets the publisher for this game
        /// </summary>
        /// <returns>PublisherResult with info about the publisher</returns>
        public async Task<PublisherResult> GetPublisherAsync()
            => await client.SendRequestAsync<PublisherResult>(RAWGClient.ENDPOINT + $"publishers/{PublisherID}");

        public override string ToString()
        {
            if (response.Error)
                return response.ToString();

            string critic = GetValue(MetaCritic);
            string release = GetValue(Released);
            string image = GetValue(ImageURI);
            string website = GetValue(Website);
            string rating = GetValue(UserRating);
            string developerID = GetValue(DeveloperID);
            string publisherID = GetValue(PublisherID);

            var trailers = GetTrailersAsync().Result;
            string _trailers = "";
            if (trailers != null)
                foreach (var trailer in trailers)
                    _trailers += "\n" + trailer.ToString();
            else _trailers = "N/A";

            var dlcs = GetTrailersAsync().Result;
            string _dlcs = "";
            if (dlcs != null)
                foreach (var dlc in dlcs)
                    _dlcs += "\n" + dlc.ToString();
            else _dlcs = "N/A";

            return $"Name: {Name}\n" +
                $"ID: {ID}\n" +
                $"Description: {Description}\n" +
                $"Meta Critic: {critic}\n" +
                $"Release: {release}\n" +
                $"Image-URI: {image}\n" +
                $"Website: {website}\n" +
                $"Developer ID: {developerID}\n" +
                $"Publisher ID: {publisherID}\n" +
                $"Rating: {rating}\n" +
                $"ESRB Rating: {ESRB}\n" +
                $"Trailer(s): {_trailers}\n" +
                $"DLC(s): {_dlcs}";
        }

        public class Rating
        {
            public int ID { get; private set; }
            public string Title { get; private set; }
            public int Count { get; private set; }
            public float Percentage { get; private set; }

            public Rating(int id, string title, int count, float percent)
            {
                ID = id;
                Title = char.ToUpper(title[0]) + title.Substring(1);
                Count = count;
                Percentage = percent;
            }

            public override string ToString()
            {
                return $" {Title} - User Ratings: {Count} ({Percentage}%)";
            }
        }
    }
}
