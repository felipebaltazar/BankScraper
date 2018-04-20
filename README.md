# BankScraper
Brazilian Bank Scraper

[![Build Status](https://travis-ci.org/felipebaltazar/BankScraper.svg?branch=master)](https://travis-ci.org/felipebaltazar/BankScraper) [![NuGet](https://img.shields.io/nuget/v/Tzar.BankScraper.svg)](https://www.nuget.org/packages/Tzar.BankScraper/)

**WARNING:** Using this tool without care may lead to your bank account being blocked. Use at your own risk!

## Current Supported Banks

The banks below were added in the order they are listed


| Name                                                                                                                                                                                          | Balance | Additional Info                                                                                       | Method                               | Status |
| ---                                                                                                                                                                                           | ---     | ---                                                                                                   | ---                                  | ---    |
| [![BancoInter](https://github.com/felipebaltazar/BankScraper/blob/master/BankScraper/Logos/Intermedium.jpg)](https://github.com/felipebaltazar/BankScraper/blob/master/BankScraper/Banks/Intermedium.cs)                              | Yes                 | Account Number, Name,                  | Reversed Web API                  | OK     |


## Nuget Install
```
Install-Package Tzar.BankScraper
```

## Usage
```csharp
            var bankscraper = new Scraper(BankFlag.Intermedium);
            var logged = bankscraper.LoginAsync(account, password).Result;

            if (logged)
                var userDetails = bankscraper.GetUserDetailsAsync().Result;
```
