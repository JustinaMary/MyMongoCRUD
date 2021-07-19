using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMongoCRUD.ServiceModels.Models
{
    public class Rating
    {
        [BsonElement("rating")]
        public double RatingScore { get; set; }
        public int NumReviews { get; set; }
        public int Meter { get; set; }
    }
}
