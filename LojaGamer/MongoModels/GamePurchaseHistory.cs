using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LojaGamer.MongoModels
{
    public class GamePurchaseHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    }
}
