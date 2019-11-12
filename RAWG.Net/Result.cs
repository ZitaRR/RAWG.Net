using System.Net;

namespace RAWG.Net
{
    public class Result
    {
        public string Name { get; private set; }
        public int? ID { get; private set; }

        private Response response = null;

        public Result(string name, int? id)
        {
            Name = name;
            ID = id;
        }

        internal Result Initialize(HttpStatusCode status)
        {
            response = new Response(status);
            return this;
        }

        public override string ToString()
        {
            if (response.Error)
                return response.ToString();
            return $"Name: {Name}\nID: {ID}";
        }
    }
}
