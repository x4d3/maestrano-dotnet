using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maestrano.Helpers
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Convert a string to snake case
        /// </summary>
        /// <param name="pascalCasedWord">String to convert</param>
        /// <returns>string</returns>
        public static string ToSnakeCase(this string word)
        {
            return
                Regex.Replace(
                    Regex.Replace(Regex.Replace(word, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
                        "$1_$2"), @"[-\s]", "_").ToLower();
        }

        public static string ToCamelCase(this string word)
        {
            // If there are 0 or 1 characters, just return the string.
            if (word == null || word.Length < 2)
                return word;

            // Split the string into words.
            string[] words = word.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

    }
}
