# BankScraper
Brazilian Bank Scraper

**WARNING:** Using this tool without care may lead to your bank account being blocked. Use at your own risk!

## Current Supported Banks

The banks below were added in the order they are listed


| Name                                                                                                                                                                                          | Balance | Transaction Backlog in Days           | Additional Info                                                                                       | Method                               | Status |
| ---                                                                                                                                                                                           | ---     | ---                                   | ---                                                                                                   | ---                                  | ---    |
| [![BancoInter](https://is4-ssl.mzstatic.com/image/thumb/Purple118/v4/7b/ff/66/7bff665b-903e-a39d-4d89-218628c5e718/AppIcon-1x_U007emarketing-0-0-GLES2_U002c0-512MB-sRGB-0-0-0-85-220-0-0-0-8.png/230x0w.jpg 1x,https://is4-ssl.mzstatic.com/image/thumb/Purple118/v4/7b/ff/66/7bff665b-903e-a39d-4d89-218628c5e718/AppIcon-1x_U007emarketing-0-0-GLES2_U002c0-512MB-sRGB-0-0-0-85-220-0-0-0-8.png/460x0w.jpg 2x,https://is4-ssl.mzstatic.com/image/thumb/Purple118/v4/7b/ff/66/7bff665b-903e-a39d-4d89-218628c5e718/AppIcon-1x_U007emarketing-0-0-GLES2_U002c0-512MB-sRGB-0-0-0-85-220-0-0-0-8.png/690x0w.jpg](https://github.com/kamushadenes/bankscraper/blob/master/itau.py)                              | Yes     | 7, 15, 30, 60, 90                     | Account Number, Owner Name, Owner Balance,                  | Reversed Web API                  | OK     |


## Usage

<pre><code>
            var bankscraper = new Scraper(BankFlag.Intermedium);
            var logged = bankscraper.LoginAsync(account, password).Result;

            if (logged)
                var userDetails = bankscraper.GetUserDetailsAsync().Result;
</code></pre>