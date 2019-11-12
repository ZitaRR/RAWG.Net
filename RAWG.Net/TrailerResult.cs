namespace RAWG.Net
{
    public class TrailerResult : Result
    {
        public string Title { get; private set; }
        public int? ID { get; private set; }
        public string URI { get; private set; }

        internal int count;
        internal dynamic[] data;

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

        internal TrailerResult[] Initialize()
        {
            if (count <= 0)
                return null;

            TrailerResult[] trailers = new TrailerResult[count];
            trailers[0] = this;
            for (int i = 1; i < count; i++)
                trailers[i] = new TrailerResult(index: i, results: data);
            return trailers;
        }

        public override string ToString()
        {
            return $"Title: {Title}\nID: {ID}\nURI: {URI}";
        }
    }
}
