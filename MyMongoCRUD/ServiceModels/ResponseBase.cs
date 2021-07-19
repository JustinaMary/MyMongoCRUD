using ServiceStack;

namespace MyMongoCRUD.ServiceModels
{
    public class ResponseBase
    {
        public ResponseStatus ResponseStatus { get; set; } = new ResponseStatus();
    }
}
