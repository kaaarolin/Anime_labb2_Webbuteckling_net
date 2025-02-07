using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Anime_labb2.Models
{
    public class Animes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }  // 🔹 Ändrat från `string` till `ObjectId`

        public string Name { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }  // 🔹 Ändrat från `string` till `int`
        public string Rating { get; set; }  // 🔹 Ändrat från `string` till `double`
    }
}
