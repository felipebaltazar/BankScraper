using BankScraper;
using BankScraper.Enums;
using System;

namespace BankScraperConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = string.Empty;
            var password = string.Empty;
            var bankscraper = new Scraper(BankFlag.Intermedium);

            Console.WriteLine("Enter your account number:");
            account = Console.ReadLine();
            Console.WriteLine("Now enter your password:");
            password = Console.ReadLine();
            
            var logged = bankscraper.LoginAsync(account, password).Result;

            if (logged)
            {
                var userDetails = bankscraper.GetUserDetailsAsync().Result;
                Console.WriteLine($"\nUser Name: {userDetails.Name}\nUser Account: {userDetails.Account}\nUser Balance: {userDetails.Balance}");
                Console.ReadKey();
            }
        }
    }
}