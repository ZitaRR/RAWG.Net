﻿using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;

namespace RAWG.Net
{
    public class GameResult : Result
    {
        public string Name { get; private set; }
        public int? ID { get; private set; }
        public string Description { get; private set; }
        public int? MetaCritic { get; private set; }
        public DateTime? Released { get; private set; }
        public string ImageURI { get; private set; }
        public string Website { get; private set; }
        public Rating UserRating { get; private set; }

        public GameResult(string name = null, int? id = null, string description = "", int? metaCritic = null,
            string released = null, string background_image = null, string website = null, params Rating[] ratings)
        {
            Name = name;
            ID = id;
            description = Regex.Replace(description, @"<[^>]*>", "");
            Description = Regex.Replace(description, @"[\.\,\-\!\?]", (c) => c.NextMatch().Value == " " ? c + " " : "");
            MetaCritic = metaCritic;
            Released = DateTime.TryParse(released, out DateTime time) ? time as DateTime? : null;
            ImageURI = background_image;
            Website = website;
            UserRating = ratings.Length > 0 ? ratings[0] : null;
        }

        public async Task<TrailerResult[]> GetTrailersAsync()
        {
            var trailers = await client.SendRequestAsync<TrailerResult>(RAWGClient.ENDPOINT + $"games/{ID}/movies");
            return trailers.Initialize();
        }

        public override string ToString()
        {
            if (response.Error)
                return response.ToString();

            string critic = GetValue(MetaCritic);
            string release = GetValue(Released);
            string image = GetValue(ImageURI);
            string website = GetValue(Website);
            string rating = GetValue(UserRating);

            var trailers = GetTrailersAsync();
            string _trailers = "";
            foreach (var trailer in trailers.Result)
                _trailers += trailer.ToString();

            return $"Name: {Name}\n" +
                $"ID: {ID}\n" +
                $"Description: {Description}\n" +
                $"Meta Critic: {critic}\n" +
                $"Release: {release}\n" +
                $"Image-URI: {image}\n" +
                $"Website: {website}\n" +
                $"Rating: {rating}\n" +
                $"Trailer(s): {_trailers}";
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
                return $"\n      Title: {Title}\n" +
                    $"      User Ratings: {Count} ({Percentage}%)\n";
            }
        }
    }
}
