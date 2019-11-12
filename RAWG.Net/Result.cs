using System;
using System.Net;
using System.Threading.Tasks;

namespace RAWG.Net
{
    public class Result
    {
        public DateTime Time { get; protected set; } = DateTime.Now;

        internal Response response;
        protected RAWGClient client;

        internal Result Initialize(HttpStatusCode status, RAWGClient client)
        {
            this.client = client;
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
    }
}
