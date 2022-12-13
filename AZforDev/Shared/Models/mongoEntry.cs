using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AZforDev.Shared.Models
{
    public class mongoEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? entry { get; set; }
    }
}
