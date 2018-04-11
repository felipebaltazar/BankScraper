using CrossBankScraperApp.Interfaces;
using MongoDB.Driver;
using System;

namespace CrossBankScraperApp.UWP.Database
{
    public class DatabaseUWP : IDatabase
    {
        private const string FILE_NAME = "ApplicationData";
        private IMongoDatabase _databaseConnection;
        private readonly MongoClient _client;

        public DatabaseUWP()
        {
            _client = new MongoClient($"mongodb://localhost:27017/{FILE_NAME}");            
        }

        public void CreateTable<T>() where T : new()
        {
            _databaseConnection = _client.GetDatabase(nameof(T));
        }

        public async void Delete<T>(T objectDto)
        {
            var collection = _databaseConnection.GetCollection<T>(nameof(T));
            await collection.DeleteOneAsync(o => (IEquatable<T>)o == (IEquatable<T>)objectDto);
        }

        public async void Save<T>(T objectDto)
        {
            var collection = _databaseConnection.GetCollection<T>(nameof(T));
            await collection.InsertOneAsync(objectDto);
        }
    }
}
