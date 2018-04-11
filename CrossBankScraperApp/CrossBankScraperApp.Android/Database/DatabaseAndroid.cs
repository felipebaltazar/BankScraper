using CrossBankScraperApp.Interfaces;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrossBankScraperApp.Droid.Database.DatabaseAndroid))]

namespace CrossBankScraperApp.Droid.Database
{
    public class DatabaseAndroid : IDatabase
    {
        private const string FILE_NAME = "ApplicationData.db3";
        private readonly SQLiteAsyncConnection _databaseConnection;

        public DatabaseAndroid()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documents, FILE_NAME);

            _databaseConnection = new SQLiteAsyncConnection(path);
        }

        public async void CreateTable<T>() where T : new()
        {
            await _databaseConnection.CreateTableAsync<T>();
        }

        public async void Delete<T>(T objectDto)
        {
            await _databaseConnection.DeleteAsync(objectDto);
        }

        public async void Save<T>(T objectDto)
        {
            await _databaseConnection.InsertOrReplaceAsync(objectDto);
        }
    }
}