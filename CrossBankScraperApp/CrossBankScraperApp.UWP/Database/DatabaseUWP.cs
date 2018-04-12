using CrossBankScraperApp.Interfaces;
using MongoDB.Driver;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrossBankScraperApp.UWP.Database.DatabaseUWP))]

namespace CrossBankScraperApp.UWP.Database
{
    public class DatabaseUWP : IDatabase
    {
        private const string FILE_NAME = "ApplicationData";
        private IMongoDatabase _databaseConnection;
        private readonly MongoClient _client;

        #region Constructors

        public DatabaseUWP()
        {
            _client = new MongoClient($"mongodb://localhost:27017/{FILE_NAME}");            
        }

        #endregion
        
        #region Public Actions

        public void CreateTable<T>() where T : new()
        {
            var objectName = GetNameOf<T>();
            _databaseConnection = _client.GetDatabase(objectName);
        }

        public async void Delete<T>(T objectDto)
        {
            var objectName = GetNameOf<T>();
            var collection = _databaseConnection.GetCollection<T>(objectName);
            await collection.DeleteOneAsync(o => (IEquatable<T>)o == (IEquatable<T>)objectDto);
        }

        public async void Save<T>(T objectDto)
        {
            var objectName = GetNameOf<T>();
            var collection = _databaseConnection.GetCollection<T>(objectName);
            await collection.InsertOneAsync(objectDto);
        }

        #endregion

        #region Local Actions

        private string GetNameOf<T>() =>
            typeof(T).Name;

        #endregion
    }
}