using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Anime_labb2.Models
{
    public class Animes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string ReleaseYear { get; set; }
        public string Rating { get; set; }
    }
}
