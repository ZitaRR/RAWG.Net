namespace RAWG.Net
{
    internal class Response
    {
        public string Message { get; private set; } = "Error";
        public bool Error { get; private set; } = true;

        public Response(string response)
        {
            switch (response)
            {
                case "Ok":
                    Message = "[200] The request was successfully completed.";
                    Error = false;
                    break;
                case "Bad Request":
                    Message = "[400] The request was invalid.";
                    break;
                case "Unauthorized":
                    Message = "[401] The request did not include an authentication token or the authentication token was expired.";
                    break;
                case "Forbidden":
                    Message = "[403] The client did not have permission to access the requested resource.";
                    break;
                case "Not Found":
                    Message = "[404] The requested resource was not found.";
                    break;
            }
        }

        public override string ToString()
            => Message;
    }
}
