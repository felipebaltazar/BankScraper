using BankScraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankScraper.Helpers
{
    internal static class BankCollectionManager
    {
        private static List<IBank> _allBanks;

        internal static List<IBank> AllBanks
        {
            get
            {
                if(_allBanks == null || _allBanks?.Count < 1)
                {
                    var interfaceType = typeof(IBank);
                    _allBanks = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                        .Select(x => Activator.CreateInstance(x) as IBank)
                        .ToList();
                }

                return _allBanks;
            }
        }
    }
}