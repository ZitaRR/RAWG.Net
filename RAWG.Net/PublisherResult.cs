using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RAWG.Net
{
    public class PublisherResult : Result
    {
        public string Name { get; }
        public int? ID { get; }
        public string ImageURI { get; }

        private List<GameResult> games;
        private ReadOnlyCollection<GameResult> gamesReadonly;
        private readonly dynamic data;

        public PublisherResult(params dynamic[] results)
        {
            Name = results[0].name;
            ID = results[0].id;
            ImageURI = results[0].image_background;
            data = results[0].top_games;
        }

        /// <summary>
        ///     Gets the top 30 games created by the publisher this class is representing
        /// </summary>
        /// <returns>An IReadOnlyCollection with the games</returns>
        public async Task<IReadOnlyCollection<GameResult>> GetGames()
        {
            if (games != null && gamesReadonly != null)
                return gamesReadonly;
            games = new List<GameResult>();
            for (int i = 0; i < data.Count; i++)
            {
                int id = data[i];
                GameResult game = await client.GetGameAsync(id);
                games.Add(game);
            }
            gamesReadonly = new ReadOnlyCollection<GameResult>(games);
            return gamesReadonly;
        }

        public override string ToString()
        {
            string _games = "";
            foreach (var game in gamesReadonly)
                _games += game.ToString() + "\n\n";

            return $"Company: {Name}\n" +
                $"ID: {ID}\n" +
                $"Image-URI: {ImageURI}\n\n" +
                $"Game(s): \n{_games}";
        }
    }
}
