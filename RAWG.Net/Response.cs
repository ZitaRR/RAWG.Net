using System.Net;

namespace RAWG.Net
{
    internal class Response
    {
        public string Message { get; } = "Error";
        public bool Error { get; } = true;

        public Response(HttpStatusCode status)
        {
            switch (status)
            {
                case HttpStatusCode.OK:
                    Message = "[200] The request was successfully completed.";
                    Error = false;
                    break;
                case HttpStatusCode.NoContent:
                    Message = "[204] The request could not retrieve any data.";
                    break;
                case HttpStatusCode.BadRequest:
                    Message = "[400] The request was invalid.";
                    break;
                case HttpStatusCode.Forbidden:
                    Message = "[403] The client did not have permission to access the requested resource.";
                    break;
                case HttpStatusCode.NotFound:
                    Message = "[404] The requested resource was not found.";
                    break;
            }
        }

        public override string ToString()
            => Message;
    }
}
