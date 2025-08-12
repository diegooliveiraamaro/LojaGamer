using LojaGamer.MongoModels;
using MongoDB.Driver;

namespace LojaGamer.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<GamePurchaseHistory> _collection;

        public MongoDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);
            _collection = database.GetCollection<GamePurchaseHistory>("PurchaseHistory");
        }

        public async Task<List<GamePurchaseHistory>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task AddAsync(GamePurchaseHistory history) =>
            await _collection.InsertOneAsync(history);
    }
}
