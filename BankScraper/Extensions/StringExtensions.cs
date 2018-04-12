using System;

namespace BankScraper.Extensions
{
    internal static class StringExtensions
    {
        public static string RemoveString(this string source, string stringToRemove) =>
            source.Replace(stringToRemove, string.Empty);

        public static string RemoveStrings(this string source, string[] stringsToRemove)
        {
            var stringResult = source;

            foreach (var stringToRemove in stringsToRemove)
                stringResult.Replace(stringToRemove, string.Empty);

            return stringResult;
        }

        public static bool EqualsIgnoreCase(this string source, string stringToCompare) =>
            source.Equals(stringToCompare, StringComparison.OrdinalIgnoreCase);
    }
}