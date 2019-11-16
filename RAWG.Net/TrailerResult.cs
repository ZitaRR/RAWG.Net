namespace RAWG.Net
{
    public class TrailerResult : Result
    {
        public string Title { get; }
        public int? ID { get; }
        public string URI { get; }

        private int count;
        private dynamic[] data;

        public TrailerResult(int count = 0, int index = 0, params dynamic[] results)
        {
            if (count <= 0)
                return;

            this.count = count;
            data = results;
            Title = data[index].name;
            ID = data[index].id;
            URI = data[index].data.max;
        }

        /// <summary>
        ///     Gets all the trailers for a game
        /// </summary>
        /// <returns>An array of trailers</returns>
        internal TrailerResult[] Initialize()
        {
            if (count <= 0)
                return null;

            var trailers = new TrailerResult[count];
            trailers[0] = this;
            for (int i = 1; i < count; i++)
                trailers[i] = new TrailerResult(count, i, data);
            return trailers;
        }

        public override string ToString()
        {
            return $"Title: {Title}\n" +
                $"ID: {ID}\n" +
                $"URI: {URI}";
        }
    }
}
