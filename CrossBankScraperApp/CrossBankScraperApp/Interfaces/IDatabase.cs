using CrossBankScraperApp.Models;

namespace CrossBankScraperApp.Interfaces
{
    public interface IDatabase
    {
        void Save<T>(T objectDto);

        void Delete<T>(T objectDto);

        void CreateTable<T>() where T : new();
    }
}
