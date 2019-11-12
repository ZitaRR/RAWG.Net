using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RAWG.Net
{
    public class DeveloperResult : Result
    {
        public string Name { get; private set; }
        public int? ID { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyCollection<GameResult> Games
        {
            get
            {
                if (games != null)
                    return new ReadOnlyCollection<GameResult>(games);
                games = new List<GameResult>();
                for (int i = 0; i < data.Count; i++)
                {
                    int id = data[i];
                    Task<GameResult> e = client.GetGameAsync(id);
                    games.Add(e.Result);
                }
                return new ReadOnlyCollection<GameResult>(games);
            }
        }
        public string ImageURI { get; private set; }

        private List<GameResult> games;
        private dynamic data;

        public DeveloperResult(params dynamic[] results)
        {
            Name = results[0].name;
            ID = results[0].id;
            ImageURI = results[0].image_background;
            data = results[0].top_games;
        }

        public override string ToString()
        {
            string _games = "";
            foreach (var game in Games)
                _games += game.ToString() + "\n\n";

            return $"Company: {Name}\n" +
                $"ID: {ID}\n" +
                $"Image-URI: {ImageURI}\n\n" + 
                $"Game(s): \n{_games}";
        }
    }
}
