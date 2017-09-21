using MongoDB.Driver;

namespace CityInfo.API.Entities
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database = null;
        private const string _connectionString = "mongodb://localhost:27017";
        private const string _dbName = "CityInfo";

        public MongoContext()
        {
            var client = new MongoClient(_connectionString);
            if (client != null)
            {
                _database = client.GetDatabase(_dbName);
            }
        }

        public IMongoCollection<MongoCity> Cities
        {
            get { return _database.GetCollection<MongoCity>("CityInfo"); }
        }
    }
}
