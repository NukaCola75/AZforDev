using MongoDB.Driver;
using System.Security.Authentication;

namespace AZforDev.Shared.Models
{
    public class mongoDbContext
    {
        private IMongoDatabase _mongoDataBase;

        public mongoDbContext()
        {
            string connectionString =
                @"mongodb://azfordevmongodb:HyZ22O6pFzztKRk8ozIoPB6wgqFrrCZesPYrn8iCC9dNO9viDydYu2974SSJzphmrmWJdV973j40ACDbGsUc4g==@azfordevmongodb.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@azfordevmongodb@";
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
          new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            _mongoDataBase = mongoClient.GetDatabase("azfordevmongodb");
        }

        public void createCollection(String collectionName)
        {
            _mongoDataBase.CreateCollection(collectionName);
        }

        public IMongoCollection<mongoEntry> getMongoEntry(string collection)
        {
            return _mongoDataBase.GetCollection<mongoEntry>(collection);
        }

        //public Task<IMongoCollection> addEntry(mongoEntry entry, string collection)
        //{
         //   return getMongoEntry(collection).InsertOneAsync(entry).Status;
        //}
    }
}
