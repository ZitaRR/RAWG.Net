using System;
using System.Net;

namespace RAWG.Net
{
    public class Result
    {
        public DateTime Time { get; protected set; } = DateTime.Now;

        internal Response response;

        internal Result Initialize(HttpStatusCode status)
        {
            response = new Response(status);
            return this;
        }

        internal string GetValue(object _object)
        {
            if (_object is null ||
                (_object is string && string.IsNullOrWhiteSpace(_object as string)))
                return "N/A";
            return _object.ToString();
        }

        internal string GetValue(DateTime? time)
        {
            if (time is null)
                return "N/A";
            return time?.ToString("yyyy-MM-dd");
        }

        public override string ToString()
        {
            return $"[{Time.ToString("HH:mm:ss")}] {response.ToString()}";
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
