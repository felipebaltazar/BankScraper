# BankScraper
Brazilian Bank Scraper

**WARNING:** Using this tool without care may lead to your bank account being blocked. Use at your own risk!

## Current Supported Banks

The banks below were added in the order they are listed


| Name                                                                                                                                                                                          | Balance | Additional Info                                                                                       | Method                               | Status |
| ---                                                                                                                                                                                           | ---     | ---                                                                                                   | ---                                  | ---    |
| [![BancoInter](https://is4-ssl.mzstatic.com/image/thumb/Purple118/v4/7b/ff/66/7bff665b-903e-a39d-4d89-218628c5e718/AppIcon-1x_U007emarketing-0-0-GLES2_U002c0-512MB-sRGB-0-0-0-85-220-0-0-0-8.png/230x0w.jpg)]                              | Yes                 | Account Number, Name,                  | Reversed Web API                  | OK     |


## Nuget Install
<pre><code>
Install-Package Tzar.BankScraper
</code></pre>

## Usage
<pre><code>
            var bankscraper = new Scraper(BankFlag.Intermedium);
            var logged = bankscraper.LoginAsync(account, password).Result;

            if (logged)
                var userDetails = bankscraper.GetUserDetailsAsync().Result;
</code></pre>
