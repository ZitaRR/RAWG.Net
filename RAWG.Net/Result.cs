namespace RAWG.Net
{
    public class Result
    {
        public string Name { get; private set; }
        public int? ID { get; private set; }

        private Response response;

        public Result(string name, int? id, string response = "Ok")
        {
            this.response = new Response(response);
            Name = name;
            ID = id;
        }

        public override string ToString()
        {
            if (response.Error)
                return response.ToString();
            return $"Name: {Name}\nID: {ID}";
        }
    }
}
